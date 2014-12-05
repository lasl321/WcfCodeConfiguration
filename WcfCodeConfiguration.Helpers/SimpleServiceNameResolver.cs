using System;

namespace WcfCodeConfiguration.Helpers
{
    public class SimpleServiceNameResolver : IServiceNameResolver
    {
        public string Resolve<T>()
        {
            var type = typeof (T);
            if (!type.IsInterface)
            {
                throw new Exception("Service type cannot be concrete class");
            }

            if (!type.Name.StartsWith("I"))
            {
                throw new Exception("Service type must follow interface conventions");
            }

            return StripLeadingCharacter(type.Name);
        }

        private static string StripLeadingCharacter(string s)
        {
            return s.Substring(1);
        }
    }
}