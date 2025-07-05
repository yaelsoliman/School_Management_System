using System.Collections.ObjectModel;

namespace Common.Authorization
{
    public static class AppRole
    {
        public const string Admin = nameof(Admin);
        public const string Teacher = nameof(Teacher);
        public const string Student = nameof(Student);

        public static IReadOnlyList<string> DefaultRoles { get; } =
            new ReadOnlyCollection<string>(new[]
            {
               Admin,
               Teacher,
               Student
            });
    
        public static bool IsDefault(string roleName) 
            => DefaultRoles.Any(x=>x == roleName);
        
    }
}
