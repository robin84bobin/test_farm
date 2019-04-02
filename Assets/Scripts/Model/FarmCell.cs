using Data.User;

namespace Model
{
    public class FarmCell 
    {
        public readonly Data.User.UserFarmCell Data;

        public FarmCell(UserFarmCell userData)
        {
            Data = userData;
        }
    }
}