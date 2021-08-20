using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DBRun
{
    class PlayProc : Main
    {
        public void PlayUpt(string inId, int roundNum)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("play_upt", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    MySqlParameter in_id = new MySqlParameter("in_id", MySqlDbType.String);
                    in_id.Direction = System.Data.ParameterDirection.Input;
                    in_id.Value = inId;
                    cmd.Parameters.Add(in_id);

                    MySqlParameter in_round = new MySqlParameter("in_round", MySqlDbType.Int16);
                    in_round.Direction = System.Data.ParameterDirection.Input;
                    in_round.Value = roundNum;
                    cmd.Parameters.Add(in_round);

                    cmd.ExecuteNonQuery();

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        LogManager.lm.Log("MySql UserRound Info Insert failure");
                        //Debug.Log("MySql UserRound Info Insert failure");
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.lm.Log("MySql UserRound Info Insert" + ex.ToString());
                //Debug.Log("MySql UserRound Info Insert failure" + ex.ToString());
            }
        }
    }

    class LoginProc : Main
    {
        public int UserIns(string inId, string inPassword)
        {
            int loginCheck = 1;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("user_ins", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    MySqlParameter in_id = new MySqlParameter("in_id", MySqlDbType.String);
                    in_id.Direction = System.Data.ParameterDirection.Input;
                    in_id.Value = inId;
                    cmd.Parameters.Add(in_id);

                    MySqlParameter in_password = new MySqlParameter("in_password", MySqlDbType.String);
                    in_password.Direction = System.Data.ParameterDirection.Input;
                    in_password.Value = inPassword;
                    cmd.Parameters.Add(in_password);

                    MySqlParameter output = new MySqlParameter("out_check", MySqlDbType.Int16);
                    output.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(output);

                    cmd.ExecuteNonQuery();

                    loginCheck = output.Value == null ? 2 :
                                Convert.ToString(output.Value) == "" ? 1 : Convert.ToInt32(output.Value);

                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        LogManager.lm.Log("MySql User Info Insert failure");
                        //Debug.Log("MySql User Info Insert failure");
                        loginCheck = 2;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.lm.Log("MySql User Info Insert : " + ex.ToString());
                //Debug.Log("MySql User Info Insert : " + ex.ToString());
                loginCheck = 2;
            }
            return loginCheck;
        }

        public DataTable UserSel(string inid, string inPassword)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    string sql = "CALL userinfo_sel('" + inid + "',  '" + inPassword + "')";
                    MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                    adpt.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                LogManager.lm.Log("MySql User Info Select : " + ex.ToString());
                //Debug.Log("MySql User Info Select" + ex.ToString());
            }
            return dt;
        }
    }
}
