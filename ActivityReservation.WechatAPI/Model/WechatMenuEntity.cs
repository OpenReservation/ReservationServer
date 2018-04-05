namespace ActivityReservation.WechatAPI.Model
{
    internal class WechatMenuEntity
    {
        public Button[] button { get; set; }
    }

    internal class Button
    {
        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public Button[] sub_button { get; set; }
    }
}
