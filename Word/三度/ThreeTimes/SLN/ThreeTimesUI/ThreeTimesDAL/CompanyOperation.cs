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
    /// 数据库访问类UserOperation 对象表Company_T
    /// </summary>
    class CompanyOperation
    {
        ThreeTimesData db = new ThreeTimesData();

        #region 数据库查询+public List<Company_T> companySelect(Expression<Func<Company_T, bool>> SelectWhere)
        /// <summary>
        /// 数据库查询
        /// </summary>
        /// <param name="SelectWhere"></param>
        /// <returns></returns>
        public List<Company_T> companySelect(Expression<Func<Company_T, bool>> SelectWhere)
        {
            return db.Company_T.Where(SelectWhere).ToList();
        } 
        #endregion

        #region 数据库查询(排序)+public List<Company_T> companySelect<TKey>(Expression<Func<Company_T, bool>> SelectWhere, Expression<Func<Company_T, TKey>> OrderLambda)
        /// <summary>
        /// 数据库查询(排序)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="SelectWhere"></param>
        /// <param name="OrderLambda"></param>
        /// <returns></returns>
        public List<Company_T> companySelect<TKey>(Expression<Func<Company_T, bool>> SelectWhere, Expression<Func<Company_T, TKey>> OrderLambda)
        {
            return db.Company_T.Where(SelectWhere).OrderBy(OrderLambda).ToList();
        } 
        #endregion

        #region 数据库查询(分页)+public List<Company_T> companySelect<TKey>(Expression<Func<Company_T, bool>> SelectWhere, Expression<Func<Company_T, TKey>> OrderLambda, int pageIndex, int pageSize)
        /// <summary>
        /// 数据库查询(分页)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="SelectWhere"></param>
        /// <param name="OrderLambda"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Company_T> companySelect<TKey>(Expression<Func<Company_T, bool>> SelectWhere, Expression<Func<Company_T, TKey>> OrderLambda, int pageIndex, int pageSize)
        {
            return db.Company_T.Where(SelectWhere).OrderBy(OrderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        } 
        #endregion
        
        #region 数据库增加+public int CompanyAdd(Company_T model)
        /// <summary>
        /// 数据库增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CompanyAdd(Company_T model)
        {
            db.Company_T.Add(model);
            return db.SaveChanges();
        } 
        #endregion

        #region 数据库删除+public int companyDel(Expression<Func<Company_T, bool>> AddWhere)
        /// <summary>
        /// 数据库删除
        /// </summary>
        /// <param name="AddWhere"></param>
        /// <returns></returns>
        public int companyDel(Expression<Func<Company_T, bool>> AddWhere)
        {
            List<Company_T> company = db.Company_T.Where(AddWhere).ToList();
            company.ForEach(d => db.Company_T.Remove(d));
            return db.SaveChanges();
        } 
        #endregion

        #region 数据库修改+public int companyModify(Company_T model, params string[] proNames)
        /// <summary>
        /// 数据库修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int companyModify(Company_T model, params string[] proNames)
        {
            DbEntityEntry entry = db.Entry<Company_T>(model);
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
