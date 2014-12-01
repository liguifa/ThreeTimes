using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTimesCommon.Models;
using System.Linq.Expressions;
namespace ThreeTimesDAL
{
    class QuestionnaireOperate
    {
        ThreeTimesData db = new ThreeTimesData();

        #region 1.0 增加 +int questionnaireAdd(Questionnarire_T modle)
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public int questionnaireAdd(Questionnarire_T modle)
        {

            db.Questionnarire_T.Add(modle);
            return db.SaveChanges();
        }
        #endregion
        #region 2.0 根据条件删除 +int questionnairreDel(Expression<Func<Questionnarire_T, bool>> whereLambda)
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public int questionnairreDel(Expression<Func<Questionnarire_T, bool>> whereLambda)
        {

            List<Questionnarire_T> listDeleting = db.Questionnarire_T.Where(whereLambda).ToList();
            listDeleting.ForEach(u => db.Questionnarire_T.Remove(u));

            return db.SaveChanges();
        }

        #endregion
        #region 3.0 根据属性修改属性值 +int questionnaireModify(Questionnarire_T modle, params string[] proNames)
        /// <summary>
        /// 根据属性修改
        /// </summary>
        /// <param name="modle"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int questionnaireModify(Questionnarire_T modle, params string[] proNames)
        {
            DbEntityEntry entry = db.Entry<Questionnarire_T>(modle);
            entry.State = System.Data.EntityState.Unchanged;
            foreach (string proName in proNames)
            {

                entry.Property(proName).IsModified = true;
            }
            return db.SaveChanges();
        }
        #endregion
        #region 4.0 查询+List<Questionnaire_T> questionnaireSelect(Expression<Func<Questionnaire_T, bool>> whereLambada)
        /// <summary>
        /// 查询 返回列表
        /// </summary>
        /// <param name="whereLambada"></param>
        /// <returns></returns>
        public List<Questionnarire_T> adminSelect(Expression<Func<Questionnarire_T, bool>> whereLambda)
        {

            return db.Questionnarire_T.Where(whereLambda).ToList();
        }
        #endregion
        #region 4.1 查询、可排序 +List<Questionnarire_T> questionnaireSelect<TKEey>(Expression<Func<Questionnarire_T, bool>> whereLambada, Expression<Func<Questionnarire_T, TKEey>> orderByLambada)
        /// <summary>
        /// 查询、可排序
        /// </summary>
        /// <typeparam name="TKEey"></typeparam>
        /// <param name="whereLambada"></param>
        /// <param name="orderByLambada"></param>
        /// <returns></returns>
        public List<Questionnarire_T> questionnaireSelect<TKEey>(Expression<Func<Questionnarire_T, bool>> whereLambada, Expression<Func<Questionnarire_T, TKEey>> orderByLambada)
        {
            return db.Questionnarire_T.Where(whereLambada).OrderBy(orderByLambada).ToList();
        } 
        #endregion
        #region 4.2 查询、返回分页数据 +List<Questionnarire_T> keySelect<TKey>(int pageIndex, int pageSize, Expression<Func<Questionnarire_T, bool>> whereLambda, Expression<Func<Questionnarire_T, TKey>> orderLambda)
        /// <summary>
        /// 查询分页数据、可排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public List<Questionnarire_T> keySelect<TKey>(int pageIndex, int pageSize, Expression<Func<Questionnarire_T, bool>> whereLambda, Expression<Func<Questionnarire_T, TKey>> orderLambda)
        {

            return db.Questionnarire_T.Where(whereLambda).OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        } 
        #endregion
    }
}
