using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Commons;
using BusinessObject.Models;
using BusinessObject.RequestModel.FeedbackRequest;
using BusinessObject.ResponseModel.FeedbackResponse;
using DataAccess.IRepositories;
using DataAccess.IServices;

namespace DataAccess.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogRepository _logRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, ILogRepository logRepository, IDeviceRepository deviceRepository, IUserRepository userRepository, IRoomRepository roomRepository)
        {
            _feedbackRepository = feedbackRepository;
            _logRepository = logRepository;
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
        }

        async Task IFeedbackService.UpdateFeedback(int id, FeedbackUpdateRequest feedbackRequest)
        {
            try
            {
                Feedback feedback = await _feedbackRepository.GetFeedback(id);

                feedback.RoomId = feedbackRequest.roomId ??= feedback.RoomId;
                feedback.Flag = feedbackRequest.flag ??= feedback.Flag;
                feedback.DeviceId = feedbackRequest.deviceId ??= feedback.DeviceId;
                feedback.Description = feedbackRequest.description ??= feedback.Description;
                feedback.Image = feedbackRequest.image ??= feedback.Image;
                feedback.Status = feedbackRequest.status ??= feedback.Status;

                if (feedback.Equals(await _feedbackRepository.GetFeedback(id)))
                {
                    //update if changed
                    feedback.UpdatedAt = DateTime.Today;
                    feedback.UpdatedBy = "system";

                    await _feedbackRepository.Update(feedback);


                    //create log
                    Log log = new()
                    {
                        FeedbackId = feedback.Id,
                        DeviceId = feedback.DeviceId,
                        Status = feedback.Status,
                        Description = feedback.Description,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "system",
                    };
                    await _logRepository.Create(log);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CreateFeedback(FeedbackRequest feedbackRequest, int userId)
        {
            //try
            //{
            User u = await _userRepository.GetUserAndDeleteIsFalse(userId);
            if (u.Status == CommonEnums.USERSTATUS.BAN)
            {
                throw new Exception("You have been banned!");
            }

            Device d = await _deviceRepository.GetDeviceAndDeleteIsFalse(feedbackRequest.deviceId);
            if (d.Status == CommonEnums.DEVICESTATUS.INACTIVE)
            {
                throw new Exception("This device is being fixed!");
            }

            Room r = await _roomRepository.GetRoomAndDeleteIsFalse(feedbackRequest.roomId);
            if(r.Status == CommonEnums.DEVICESTATUS.INACTIVE)
            {
                throw new Exception("This room is not available!");
            }

            if(feedbackRequest.description == null)
            {
                throw new Exception("You must describe the condition!");
            }

            Feedback feedback = new()
            {
                UserId = userId,
                RoomId = feedbackRequest.roomId,
                DeviceId = feedbackRequest.deviceId,
                Description = feedbackRequest.description,
                Image = feedbackRequest.image,
                Status = CommonEnums.FEEDBACKSTATUS.PENDING,
                IsDeleted = false,
                CreatedAt = DateTime.Today,
                CreatedBy = userId.ToString(),

            };
            await _feedbackRepository.Create(feedback);

            //create log
            Log log = new()
            {
                FeedbackId = feedback.Id,
                DeviceId = feedback.DeviceId,
                Status = CommonEnums.LOGSTATUS.FEEDBACK_CREATE,
                Description = feedback.Description,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "system",
            };
            await _logRepository.Create(log);

            return feedback.Id;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        async Task<IEnumerable<FeedbackResponse>> IFeedbackService.GetAllFeedback()
        {
            try
            {
                IEnumerable<Feedback> feedbacks = await _feedbackRepository.GetList();
                IEnumerable<FeedbackResponse> FeedbackResponses = feedbacks
                    .Select(f => Feedback.MapToResponse(f));
                return FeedbackResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task<FeedbackResponse> IFeedbackService.GetFeedbackById(int id)
        {
            try
            {
                Feedback feedback = await _feedbackRepository.GetFeedback(id);
                return new FeedbackResponse() {
                    id = feedback.Id,
                    userName = feedback.User.FullName,
                    roomName = feedback.Room.Name,
                    deviceName = feedback.Device.Name,
                    description = feedback.Description,
                    status = feedback.Status.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async Task IFeedbackService.DeleteFeedback(int id)
        {
            try
            {
                await _feedbackRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DenyFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedback(id);
                if (feedback.Status == CommonEnums.FEEDBACKSTATUS.PENDING)
                {
                    feedback.Status = CommonEnums.FEEDBACKSTATUS.DENY;
                }
                else
                {
                    throw new Exception("Feedback not on Pending status !!!");
                }
                await _feedbackRepository.Update(feedback);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_DENY,
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AcceptFeedback(int id, int fixerId)
        {
            try
            {
                var fixer = await _userRepository.GetUserAndDeleteIsFalse(fixerId);
                if (fixer.Role.GetValueOrDefault() != CommonEnums.ROLE.FIXER)
                {
                    throw new Exception("User not a fixer !!!");
                }
                var feedback = await _feedbackRepository.GetFeedback(id);
                if (feedback.Status == CommonEnums.FEEDBACKSTATUS.PENDING)
                {
                    feedback.Status = CommonEnums.FEEDBACKSTATUS.ACCEPT;
                } else
                {
                    throw new Exception("Feedback not on Pending status !!!");
                }

                await _feedbackRepository.Update(feedback);

                //update device status
                var device = await _deviceRepository.GetDeviceAndDeleteIsFalse(feedback.DeviceId);
                device.Status = CommonEnums.DEVICESTATUS.INACTIVE;
                await _deviceRepository.UpdateDevice(device);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    FixerId = fixerId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_ACCEPT,
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task CloseFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedback(id);
                if (feedback.Status == CommonEnums.FEEDBACKSTATUS.PENDING)
                {
                    feedback.Status = CommonEnums.FEEDBACKSTATUS.CLOSE;
                }
                else
                {
                    throw new Exception("Feedback not on Pending status !!!");
                }
                await _feedbackRepository.Update(feedback);

                //update device status
                var device = await _deviceRepository.GetDeviceAndDeleteIsFalse(feedback.DeviceId);
                device.Status = CommonEnums.DEVICESTATUS.ACTIVE;
                await _deviceRepository.UpdateDevice(device);

                //create log
                Log log = new()
                {
                    FeedbackId = feedback.Id,
                    DeviceId = feedback.DeviceId,
                    Status = CommonEnums.LOGSTATUS.FEEDBACK_CLOSE,
                    Description = feedback.Description,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                };
                await _logRepository.Create(log);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackByUserId(int userId)
        {
            var feedbacks = await _feedbackRepository.GetList();
            return feedbacks.Where(f => f.UserId.GetValueOrDefault().Equals(userId)).Select(f => Feedback.MapToResponse(f));
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacksByFixerIdAndStatusIsAccept(int id)
        {
            // Get logs of fixer with status Accpet and Close 
            var logs = await _logRepository.GetLogsByFixerId(id);
            if(logs == null)
            {
                throw new Exception("Fixer has no feedback.");
            }

            List<Log> fbClose = new List<Log>();
            List<Log> fbAccept = new List<Log>();
            List<Feedback> feedbacks = new List<Feedback>();

            // Get logs with status Accept
            foreach (var l in logs.ToList())
            {
                if(l.Status == CommonEnums.LOGSTATUS.FEEDBACK_ACCEPT)
                {
                    fbAccept.Add(l);    
                }
            }

            // Get logs with status Close
            foreach (var l1 in logs.ToList())
            {
                if (l1.Status == CommonEnums.LOGSTATUS.FEEDBACK_CLOSE)
                {
                    fbClose.Add(l1);
                }
            }

            // Remove logs with status Close which has identical Feedback Id
            foreach (var l2 in fbAccept.ToList())
            {
                foreach (var l3 in fbClose.ToList())
                {
                    if(l2.FeedbackId == l3.FeedbackId)
                    {
                        fbAccept.Remove(l2);
                    }
                }
            }

            //if(fbAccept.Count == 0)
            //{
            //    throw new Exception("Fixer has no Accept feedbacks.");
            //}

            foreach (var log in fbAccept.ToList())
            {
                feedbacks.Add(log.Feedback);
            }

             IEnumerable<FeedbackResponse> results = feedbacks.Select(
                feedback =>
                {
                    var u =  _userRepository.GetUserAndDeleteIsFalseNoTask(feedback.UserId ?? default(int));
                    var r =  _roomRepository.GetRoomAndDeleteIsFalseNoTask(feedback.RoomId);
                    var d =  _deviceRepository.GetDeviceAndDeleteIsFalseNoTask(feedback.DeviceId);
                    string statuss = "ACCEPT";

                    return new FeedbackResponse()
                    {
                        id = feedback.Id,
                        userName = u.FullName,
                        roomName = r.Name,
                        deviceName = d.Name,
                        description = feedback.Description,
                        status = statuss
                    };
                }
                )
                .ToList();
            return results;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetFeedbacksByFixerIdAndStatusIsClose(int id)
        {
            // Get logs of fixer with status Accpet and Close 
            var logs = await _logRepository.GetLogsByFixerId(id);
            if (logs == null)
            {
                throw new Exception("Fixer has no feedback.");
            }

            List<Log> fbClose = new List<Log>();
            List<Feedback> feedbacks = new List<Feedback>();

            // Get logs with status Close
            foreach (var l1 in logs.ToList())
            {
                if (l1.Status == CommonEnums.LOGSTATUS.FEEDBACK_CLOSE)
                {
                    fbClose.Add(l1);
                }
            }

            //if(fbClose.Count == 0)
            //{
            //    throw new Exception("Fixer has no Close feedbacks.");
            //}

            foreach (var log in fbClose.ToList())
            {
                feedbacks.Add(log.Feedback);
            }

            IEnumerable<FeedbackResponse> results = feedbacks.Select(
               feedback =>
               {
                   var u = _userRepository.GetUserAndDeleteIsFalseNoTask(feedback.UserId ?? default(int));
                   var r = _roomRepository.GetRoomAndDeleteIsFalseNoTask(feedback.RoomId);
                   var d = _deviceRepository.GetDeviceAndDeleteIsFalseNoTask(feedback.DeviceId);
                   string statuss = "CLOSE";

                   return new FeedbackResponse()
                   {
                       id = feedback.Id,
                       userName = u.FullName,
                       roomName = r.Name,
                       deviceName = d.Name,
                       description = feedback.Description,
                       status = statuss
                   };
               }
               )
               .ToList();
            return results;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackOnPending()
        {
            var feedbacks = await _feedbackRepository.GetList();
            return feedbacks.Where(f => f.Status == CommonEnums.FEEDBACKSTATUS.PENDING).Select(f => Feedback.MapToResponse(f));
        }

        public async Task ProcessFeedback(int fixerId, int feedbackId, string message)
        {
            Feedback fb = await _feedbackRepository.GetFeedback(feedbackId);
            if (fb == null || fb.Status != CommonEnums.FEEDBACKSTATUS.ACCEPT)
            {
                throw new Exception("This feedback is unavailable to process.");
            }

            Log log = await _logRepository.GetLogByFeedbackIdAndLogStatusIsAccept(feedbackId);
            if(log == null)
            {
                throw new Exception("Feedback's Log cannot be found!");
            }

            User fixer = await _userRepository.GetUserAndDeleteIsFalse(log.FixerId ?? default(int));
            if (fixer == null)
            {
                throw new Exception("Fixer not found");                        
            }
            if (fixer.Department.Status == CommonEnums.DEPARTMENTSTATUS.BUSY)
            {
                throw new Exception("Department busy!");
            }

            //update device status
            var device = fb.Device;
            device.Status = CommonEnums.DEVICESTATUS.ACTIVE;
            

            Log newLog = new()
            {
                FeedbackId = fb.Id,
                DeviceId = fb.DeviceId,
                FixerId = log.FixerId,
                Status = CommonEnums.LOGSTATUS.FEEDBACK_CLOSE,
                Description = message,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "system",
            };

            fb.Status = CommonEnums.FEEDBACKSTATUS.CLOSE;
            fb.UpdatedAt = DateTime.Now;
            fb.UpdatedBy = "Fixer";

            await _deviceRepository.UpdateDevice(device);
            await _feedbackRepository.Update(fb);
            await _logRepository.Create(newLog);
        }
    }
}

