namespace Model.Request.Base
{
    public class HasUserIdRequest
    {
        private int user_id { get; set; }
        public void SetUserId(int user_id)
        {
            this.user_id = user_id;
        }
        public int GetUserId()
        {
            return this.user_id;
        }
    }
}