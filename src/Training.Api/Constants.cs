namespace Training.Api
{
    public static class Constants
    {
        // Generate new one using any one of the following
        // https://wefreestar.com/toolbox/key-generator?lang=en-US - select "AES" then "Base64" then "256bits"
        // https://www.digitalsanctuary.com/aes-key-generator-free
        // https://acte.ltd/utils/randomkeygen - copy "Encryption key 256" then use "btoa" method of javascript to convert into base64
        public const string JwtSecretKey = "VW9AdrEpnO4AWbNp2i7FbHeSMVeXNKQ7N64jrJgMbO4=";
        public const string JwtAudiance = "Training";
        public const string JwtIssuer = "Training_API";
        

    }
}

