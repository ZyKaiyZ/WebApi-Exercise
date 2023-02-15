using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using MySql.Data.MySqlClient;

namespace Exercise.ClassLibrary
{
    public class Multiplication
    {
        public Multiplication(int multiplicandParameter, int multiplierParameter)
        {
            Multiplicand = multiplicandParameter;
            Multiplier = multiplierParameter;
            Product = multiplicandParameter * multiplierParameter;
            Formula = $"{Multiplicand} * {Multiplier} = " + String.Format("{0,2}", Product);
        }
        public int Multiplicand { get; set; } //被乘數
        public int Multiplier { get; set; } //乘數
        public int Product { get; set; } //積
        public String Formula { get; set; } //算式
        public ConsoleColor Color { get; set; }
        public void PrintFormula()
        {
            Console.Write(Formula + "    ");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    public class GetMultiplicationTableResult
    {
        public GetMultiplicationTableResult(int multiplicandParameter)
        {
            Multiplicand = multiplicandParameter;
            Multiplications = new List<Multiplication>();
            for (int i = 1; i < 10; i++)
            {
                Multiplications.Add(new Multiplication(multiplicandParameter, i));
            }
        }
        public int Multiplicand { get; set; } //被乘數
        public List<Multiplication> Multiplications { get; set; }
    }
    public class GetMultiplicationTableParameter
    {
        public int Multiplicand { get; set; }
    }
    public class ReturnData<T> where T : class
    {
        public int Code { get; set; }
        public string? Status { get; set; }
        public T? Data { get; set; }
    }
    /// <summary>
    /// 系統參數檔
    /// </summary>
    [Serializable]
    [Table("tb_wa_lookup_code")]
    [Description("系統參數檔")]
    public sealed class LookupCode
    {
        /// <summary>
        /// 系統參數ID
        /// </summary>
        [Key]
        [Column("lookup_id", TypeName = "bigint")]
        [Description("系統參數ID")]
        [JsonPropertyName("lookup_id")]
        public long LookupId { get; set; }

        /// <summary>
        /// 系統參數類型
        /// </summary>
        [Required]
        [DefaultValue("")]
        [IndexColumn("UX_WA_lookupCode", IsUnique = true)]
        [Column("lookup_type", TypeName = "varchar(20)")]
        [Description("系統參數類型")]
        [JsonPropertyName("lookup_type")]
        public string LookupType { get; set; }

        [Required]
        [DefaultValue("")]
        [IndexColumn("UX_WA_lookupCode", IsUnique = true)]
        [Column("lookup_code", TypeName = "varchar(20)")]
        [Description("系統參數")]
        [JsonPropertyName("lookup_code")]
        public string Code { get; set; }

        /// <summary>
        /// 參數說明
        /// </summary>
        [Required]
        [DefaultValue("")]
        [Column("description", TypeName = "varchar(200)")]
        [Description("參數說明")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// 數值(可用來排序)
        /// </summary>
        [IndexColumn("IX_WA_lookupValue")]
        [Column("lookup_value", TypeName = "smallint")]
        [Description("數值(可用來排序)")]
        [JsonPropertyName("lookup_value")]
        public short LookupValue { get; set; }

        /// <summary>
        /// 文字欄1
        /// </summary>
        [IndexColumn("IX_WA_text1")]
        [Column("text1", TypeName = "varchar(200)")]
        [Description("文字欄1")]
        [JsonPropertyName("text1")]
        public string Text1 { get; set; }

        /// <summary>
        /// 文字欄2
        /// </summary>
        [IndexColumn("IX_WA_text2")]
        [Column("text2", TypeName = "varchar(200)")]
        [Description("文字欄2")]
        [JsonPropertyName("text2")]
        public string Text2 { get; set; }

        /// <summary>
        /// 文字欄3
        /// </summary>
        [IndexColumn("IX_WA_text3")]
        [Column("text3", TypeName = "varchar(200)")]
        [Description("文字欄3")]
        [JsonPropertyName("text3")]
        public string Text3 { get; set; }

        /// <summary>
        /// 是否為系統內部使用(不開放UI修改)
        /// </summary>
        [Required]
        [DefaultValue("N")]
        [Column("is_system_use", TypeName = "varchar(1)")]
        [Description("是否為系統內部使用(不開放UI修改)")]
        [JsonPropertyName("is_system_use")]
        public string IsSystemUse { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Column("remark", TypeName = "varchar(2000)")]
        [Description("備註")]
        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否能被刪除 Y/N
        /// </summary>
        [Column("del_flag", TypeName = "varchar(5)")]
        [Description("是否能被刪除")]
        [JsonPropertyName("del_flag")]
        public string DeletedFlag { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        [Required]
        [DefaultValue("")]
        [Column("cr_user", TypeName = "varchar(100)")]
        [Description("建立人員")]
        [JsonPropertyName("cr_user")]
        public string CreatedUser { get; set; }

        /// <summary>
        /// 建立日期(用timestamp格式寫入)
        /// </summary>
        [Required]
        [DefaultValue(0)]
        [Column("cr_date", TypeName = "bigint")]
        [Description("建立日期(用timestamp格式寫入)")]
        [JsonPropertyName("cr_date")]
        public long CreatedDateTime { get; set; }

        /// <summary>
        /// 最後更新人員
        /// </summary>
        [Required]
        [DefaultValue("")]
        [Column("upd_user", TypeName = "varchar(100)")]
        [Description("最後更新人員")]
        [JsonPropertyName("upd_user")]
        public string UpdatedUser { get; set; }

        /// <summary>
        /// 最後更新日期(用timestamp格式寫入)
        /// </summary>
        [Required]
        [DefaultValue(0)]
        [Column("upd_date", TypeName = "bigint")]
        [Description("最後更新日期(用timestamp格式寫入)")]
        [JsonPropertyName("upd_date")]
        public long UpdatedDateTime { get; set; }
    }
    public class LookupCodeService
    {
       public LookupCodeService(MySqlConnection mySqlConnection)
        {
            LookupCodes=new List<LookupCode>();
            using (mySqlConnection)
            {
                mySqlConnection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM tb_wa_lookup_code2;", mySqlConnection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LookupCodes.Add(new LookupCode()
                            {
                                LookupId = Convert.ToInt32(reader["lookup_id"].ToString()),
                                LookupType = reader["lookup_type"].ToString(),
                                Code = reader["lookup_code"].ToString(),
                                Description = reader["description"].ToString(),
                                LookupValue = (short)Convert.ToInt32(reader["lookup_value"].ToString()),
                                DeletedFlag = reader["del_flag"].ToString(),
                                CreatedUser = reader["cr_user"].ToString(),
                                CreatedDateTime = Convert.ToInt32(reader["cr_date"].ToString()),
                                UpdatedUser = reader["upd_user"].ToString(),
                                UpdatedDateTime = Convert.ToInt32(reader["upd_date"].ToString()),
                            });
                        }
                    }
                }
                mySqlConnection.Close();
            }
        }
        public List<LookupCode> LookupCodes { get; set; }
        public ConsoleColor GetConsoleColor(int productParameter)
        {
            ConsoleColor result = ConsoleColor.White;
            int maxNumber = 0;
            foreach (LookupCode item in LookupCodes)
            {
                if (productParameter % Convert.ToInt32(item.Code) == 0 && maxNumber < Convert.ToInt32(item.Code))
                {
                    maxNumber = Convert.ToInt32(item.Code);
                    result = (ConsoleColor)item.LookupValue;
                }
            }
            return result;
        }
    }
}