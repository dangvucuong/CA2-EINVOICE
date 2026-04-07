using Swashbuckle.AspNetCore.Annotations;

namespace Model.Base
{
    public class modify_infor
    {
        // [JsonIgnore]
        [SwaggerSchema(Description = "Bỏ qua")]
        public bool is_deleted { get; set; }
        // [JsonIgnore]
        [SwaggerSchema(Description = "Bỏ qua")]
        public DateTime created_time { get; set; }
        // [JsonIgnore]
        [SwaggerSchema(Description = "Bỏ qua")]
        public int created_user_id { get; set; }
        [SwaggerSchema(Description = "Bỏ qua")]
        // [JsonIgnore]
        public DateTime last_modified_times { get; set; }
        [SwaggerSchema(Description = "Bỏ qua")]
        // [JsonIgnore]
        public int last_modified_user_id { get; set; }

        /// <summary>
        /// Set thông tin người thêm, thời gian thêm record này
        /// </summary>
        /// <param name="UserID"></param>
        public void SetInsertInfo(int created_user_id)
        {
            this.is_deleted = false;
            this.created_user_id = this.last_modified_user_id = created_user_id;
            this.created_time = this.last_modified_times = DateTime.Now;
        }
        /// <summary>
        /// Set thông tin người sửa, thời gian sửa của record này
        /// </summary>
        /// <param name="UserID"></param>
        public void SetUpdateInfo(int last_modified_user_id)
        {
            this.last_modified_user_id = last_modified_user_id;
            this.last_modified_times = DateTime.Now;
        }
    }
}

