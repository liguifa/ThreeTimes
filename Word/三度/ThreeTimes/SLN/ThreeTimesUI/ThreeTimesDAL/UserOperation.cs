using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThreeTimesCommon.Models;


namespace ThreeTimesDAL
{
	/// <summary>
	/// 数据库访问类UserOperation 对象表User_T
	/// </summary>
	class UserOperation
	{
		ThreeTimesData db = new ThreeTimesData();

		#region 数据库查询+public List<User_T> userSelect(Expression<Func<User_T, bool>> SelectWhere)
		/// <summary>
		/// 数据库查询
		/// </summary>
		/// <param name="SelectWhere"></param>
		/// <returns></returns>
		public List<User_T> userSelect(Expression<Func<User_T, bool>> SelectWhere)
		{
			return db.User_T.Where(SelectWhere).ToList();
		} 
		#endregion

		#region 数据库查询(排序)+ public List<User_T> userSelect<TKey>(Expression<Func<User_T,bool>> SelectWhere,Expression<Func<User_T,TKey>> OrderLambda)
		/// <summary>
		/// 数据库查询(排序)
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="SelectWhere"></param>
		/// <param name="OrderLambda"></param>
		/// <returns></returns>
		public List<User_T> userSelect<TKey>(Expression<Func<User_T,bool>> SelectWhere,Expression<Func<User_T,TKey>> OrderLambda)
		{
			return db.User_T.Where(SelectWhere).OrderBy(OrderLambda).ToList();
		} 
	#endregion

		#region 数据库查询(分页)+public List<User_T> userSelect<TKey>(Expression<Func<User_T,bool>> SelectWhere,Expression<Func<User_T,TKey>> OrderLambda,int pageIndex,int pageSize)
		/// <summary>
		/// 数据库查询(分页)
		/// </summary>
		/// <param name="OrderLambda"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<User_T> userSelect<TKey>(Expression<Func<User_T,bool>> SelectWhere,Expression<Func<User_T,TKey>> OrderLambda,int pageIndex,int pageSize)
		{
			return db.User_T.Where(SelectWhere).OrderBy(OrderLambda).Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
		} 
	#endregion

		#region 数据库添加+public int userAdd(User_T model)
		/// <summary>
		/// 数据库添加
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int userAdd(User_T model)
		{
			db.User_T.Add(model);
			return db.SaveChanges();
		}
		
		#endregion

		#region 数据库删除+public int userDel(Expression<Func<User_T, bool>> DelWhere)
		/// <summary>
		/// 数据库删除
		/// </summary>
		/// <param name="DelWhere"></param>
		/// <returns></returns>
		public int userDel(Expression<Func<User_T, bool>> DelWhere)
		{
			List<User_T> user = db.User_T.Where(DelWhere).ToList();
			user.ForEach(d => db.User_T.Remove(d));
			return db.SaveChanges();
		} 
		#endregion

		#region 数据库修改+ public int userModify(User_T model, params string[] proNames)
		/// <summary>
		/// 数据库修改
		/// </summary>
		/// <param name="model"></param>
		/// <param name="proNames"></param>
		/// <returns></returns>
		public int userModify(User_T model, params string[] proNames)
		{
			DbEntityEntry entry = db.Entry<User_T>(model);
			entry.State = EntityState.Unchanged;
			foreach (string proName in proNames)
			{
				entry.Property(proName).IsModified = true;
			}

			return db.SaveChanges();
		} 
		#endregion
	}
}
