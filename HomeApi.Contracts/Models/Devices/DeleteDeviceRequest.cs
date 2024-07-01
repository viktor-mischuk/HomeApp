

namespace HomeApi.Contracts.Models.Devices
{
    public class DeleteDeviceRequest
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Room { get; set; }

    }
}
