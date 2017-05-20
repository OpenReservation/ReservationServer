using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;

namespace Common
{
    /// <summary>
    /// 类型转换助手
    /// </summary>
    public static class ConverterHelper
    {
        /// <summary>  
        /// 利用反射和泛型  
        /// </summary>  
        /// <param name="dt">DataTable 对象</param>  
        /// <returns></returns>  
        public static List<T> DataTableToList<T>(DataTable dt) where T :class,new ()
       {  
           // 定义集合  
           List<T> ts = new List<T>();  
 
           // 获得此模型的类型  
           Type type = typeof(T);  
           //定义一个临时变量  
           string tempName = string.Empty;  
           //遍历DataTable中所有的数据行  
           foreach (DataRow dr in dt.Rows)  
           {  
               T t = new T();  
               // 获得此模型的公共属性  
               PropertyInfo[] propertys = t.GetType().GetProperties();  
               //遍历该对象的所有属性  
               foreach (PropertyInfo pi in propertys)  
               {  
                   tempName = pi.Name;//将属性名称赋值给临时变量  
                   //检查DataTable是否包含此列（列名==对象的属性名）    
                   if (dt.Columns.Contains(tempName))  
                   {  
                       
                       //取值  
                       object value = dr[tempName];  
                       //如果非空，则赋给对象的属性  
                       if (value != DBNull.Value)  
                           pi.SetValue(t,value,null);  
                   }  
               }  
               //对象添加到泛型集合中  
               ts.Add(t);  
           }  
 
           return ts;  
 
       }

        /// <summary>
        /// 将object对象转换为Json数据
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <returns>转换后的json字符串</returns>
        public static string ObjectToJson(object obj)
        {
            if(obj == null)
            {
                return "";
            }
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将Json对象转换为T对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="jsonString">json对象字符串</param>
        /// <returns>由字符串转换得到的T对象</returns>
        public static T JsonToObject<T>(string jsonString)
        {
            if(String.IsNullOrEmpty(jsonString))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 从字符串中获取数据
        /// </summary>
        /// <param name="content">源字符串</param>
        /// <returns>字符串中的值</returns>
        public static string GetContent(string content)
        {
            return (String.IsNullOrEmpty(content) ? null : content);
        }

        /// <summary>
        /// 将int转换为bool类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertIntToBool(int value)
        {
            return (value > 0 ? true : false);
        }
    }   
}