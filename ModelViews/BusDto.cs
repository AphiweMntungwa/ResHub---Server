namespace ResHub.ModelViews
{
    public class BusDto
    {
        public string BusNumber { get; set; }
        public List<string> FromTimes { get; set; }  // ISO 8601 format strings (e.g., '2024-09-07T05:15:00.000Z')
        public List<string> ToTimes { get; set; }
        public string BusDriver { get; set; }
        public string BusDriverPhoneNumber { get; set; }
    }

}
