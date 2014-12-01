using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTimesCommon.Models;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace ThreeTimesDAL
{
    /// <summary>
    /// 数据库访问类QuestionClassOperation 对象表QuestionClass_T
    /// </summary>
    class QuestionClassOperation
    {
        ThreeTimesData db = new ThreeTimesData();

        #region 数据库查询+ public List<QuestionClass_T> questionclassSelectExpression<Func<QuestionClass_T, bool>> SelectWhere)
        /// <summary>
        /// 数据库查询
        /// </summary>
        /// <param name="SelectWhere"></param>
        /// <returns></returns>
        public List<QuestionClass_T> questionclassSelect(Expression<Func<QuestionClass_T, bool>> SelectWhere)
        {
            return db.QuestionClass_T.Where(SelectWhere).ToList();
        } 
        #endregion

        #region 数据库查询(排序)+public List<QuestionClass_T> questionclassSelect<TKey>(Expression<Func<QuestionClass_T, bool>> SelectWhere, Expression<Func<QuestionClass_T, TKey>> OrderLambda)
        /// <summary>
        /// 数据库查询(排序)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="SelectWhere"></param>
        /// <param name="OrderLambda"></param>
        /// <returns></returns>
        public List<QuestionClass_T> questionclassSelect<TKey>(Expression<Func<QuestionClass_T, bool>> SelectWhere, Expression<Func<QuestionClass_T, TKey>> OrderLambda)
        {
            return db.QuestionClass_T.Where(SelectWhere).OrderBy(OrderLambda).ToList();
        } 
        #endregion

        #region 数据库查询(分页)+public List<QuestionClass_T> questionclassSelect<TKey>(Expression<Func<QuestionClass_T, bool>> SelectWhere, Expression<Func<QuestionClass_T, TKey>> OrderLambda, int pageIndex, int pageSize)
        /// <summary>
        /// 数据库查询(分页)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="SelectWhere"></param>
        /// <param name="OrderLambda"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<QuestionClass_T> questionclassSelect<TKey>(Expression<Func<QuestionClass_T, bool>> SelectWhere, Expression<Func<QuestionClass_T, TKey>> OrderLambda, int pageIndex, int pageSize)
        {
            return db.QuestionClass_T.Where(SelectWhere).OrderBy(OrderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        } 
        #endregion

        #region 数据库增加+public int questionclassAdd(QuestionClass_T model)
        /// <summary>
        /// 数据库增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int questionclassAdd(QuestionClass_T model)
        {
            db.QuestionClass_T.Add(model);
            return db.SaveChanges();
        } 
        #endregion

        #region 数据库删除+ public int questionclassDel(Expression<Func<QuestionClass_T, bool>> DelWhere)
        /// <summary>
        /// 数据库删除
        /// </summary>
        /// <param name="DelWhere"></param>
        /// <returns></returns>
        public int questionclassDel(Expression<Func<QuestionClass_T, bool>> DelWhere)
        {
            List<QuestionClass_T> questionclass = db.QuestionClass_T.Where(DelWhere).ToList();
            questionclass.ForEach(d => db.QuestionClass_T.Remove(d));
            return db.SaveChanges();
        } 
        #endregion

        #region 数据库修改+public int questionclassModify(QuestionClass_T model, params string[] proNames)
        /// <summary>
        /// 数据库修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int questionclassModify(QuestionClass_T model, params string[] proNames)
        {
            DbEntityEntry entry=db.Entry<QuestionClass_T>(model);
            entry.State = EntityState.Unchanged;
            foreach(string proName in proNames)
            {
                entry.Property(proName).IsModified=true;
            }
           return db.SaveChanges();
        } 
        #endregion
    }
}
