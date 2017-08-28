namespace ActivityReservation.HelperModels
{
    public class JsonResultModel
    {
        public JsonResultStatus Status { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }

    public class JsonResultModel<T>
    {
        public JsonResultStatus Status { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }

    public enum JsonResultStatus
    {
        Success = 200,                
        RequestError = 400,
        NoPermission = 403,
        ResourceNotFound = 404,
        ProcessFail = 500,
    }
}
