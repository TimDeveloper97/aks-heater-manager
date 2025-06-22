namespace VstCommon
{
    /// <summary>
    /// https://bks.vst.edu.vn/Admin
    /// </summary>
    public class API
    {
        public const string BASE_URL = "https://bks.vst.edu.vn/api/";
        public static string UserType = "customer";
        public const string CUSTOMER = "customer";

        /// <summary>
        /// {
        ///  "value": {
        ///    "_id": "0394852798",
        ///    "role": "Customer",
        ///    "name": "Duy Anh",
        ///    "timeout": 120,
        ///    "token": "89e2fba7b5dcc32d16dfe18748748ca3",
        ///    "staff": {
        ///      "0123456": {}
        ///    },
        ///    "device": {
        ///      "000006": {}
        ///    }
        ///  }
        ///}
        /// </summary>
        public static string Login = BASE_URL + "guest/login";

        /// <summary>
        /// {
        ///  "value": [
        ///    {
        ///      "_id": "000006",
        ///      "name": "Demo",
        ///      "addr": "Trung Sam",
        ///      "model": "ELECTRIC",
        ///      "version": "1.0"
        ///    }
        ///  ]
        ///}
        /// </summary>
        public static string DeviceList = BASE_URL + $"{UserType}/device-list";

        /// <summary>
        /// {
        ///  "value": [
        ///    {
        ///      "_id": "0123456",
        ///      "password": "e5f02d42fc78401c7ce1dadbc2fbcfe4",
        ///      "role": "Staff",
        ///      "name": "ABC"
        ///    }
        ///  ]
        ///}
        /// </summary>
        public static string UserList = BASE_URL + $"{UserType}/user-list";

        /// <summary>
        /// {
        ///  "value": {
        ///    "_id": "000006",
        ///    "U": 233,
        ///    "I": 0,
        ///    "C": 0.44
        ///  }
        ///}
        /// </summary>
        public static string DeviceStatus = BASE_URL + $"{UserType}/device-status";
    }
}
