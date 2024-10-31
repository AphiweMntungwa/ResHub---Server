namespace ResHub.ModelViews
{
    public class BusDto
    {
        public string BusNumber { get; set; }
        public List<string> FromTimes { get; set; } 
        public List<string> ToTimes { get; set; }
        public string BusDriver { get; set; }
        public string BusDriverPhoneNumber { get; set; }
    }

}
