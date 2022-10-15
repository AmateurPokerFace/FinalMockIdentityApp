namespace FinalMockIdentityXCountry.Models.Utilities
{
    public class StaticDetails
    {
        // Will have all privileges
        public const string Role_Master_Admin = "Master Admin"; 
        //public const string Role_Admin = "Admin"; 

        // Will have all privileges other than removing a coach from the list of coaches and adding a coach to the revoked role
        public const string Role_Coach = "Coach"; 
        
        public const string Role_Runner = "Runner";

        // Will be the default role assigned. Must be approved by coach/admin before being allowed to do CRUD operations
        public const string Role_Not_Assigned = "Waiting for approval";

        // will be used for banned users
        public const string Role_Was_Revoked= "Banned user"; 
    }
}
