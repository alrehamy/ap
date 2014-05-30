using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public class Affinity_propagation
{
    public const int num = 10;
    //定义点集 define matrix
    public PointF[] points = new PointF[num];
    //判定是否为中心点 define center point
    public int[] iscenter = new int[num];
    //定义相似度矩阵
    public double[,] similarmatrix = new double[num, num];
    //定义消息
    public double[,] msga = new double[num, num];
    public double[,] msgr = new double[num, num];
    public double[,] oldmsga = new double[num, num];
    public double[,] oldmsgr = new double[num, num];
    //定义中值
    public double pk = 0;
    //定义阻尼系数
    public const double dampcv = 0.5;
    public Affinity_propagation()
	{
        InitializeComponent();
        InitPoints();
        InitMsga();

        ///第一步，创建相似度矩阵
        CreateSimilarMatrix(); 

	}
 
 
    /// <summary>
    /// 创建相似度矩阵.这里有点疑惑，是不是先按照常规计算再去算Pk？？
    /// 而且这个中值是不是要排除0？先不管它，当不排除
    /// </summary>
    private void CreateSimilarMatrix()
    {
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                similarmatrix[i, j] = CalDistant(points[i], points[j]);
            }
        }
        double median = ReturnMedian(similarmatrix);
        pk = median / 2;                                         //取中值作为preference的值
        for (int i = 0; i < num; i++)
        {
            similarmatrix[i, i] = pk;
        }
    }
    
    /// <summary>
        /// 初始化点集
        /// </summary>
        private void InitPoints()
        {
            points[0].X = (float)0.1;
            points[0].Y = (float)0.1;
            points[1].X = (float)0.3;
            points[1].Y = (float)0.1;
            points[2].X = (float)0.3;
            points[2].Y = (float)0.3;
            points[3].X = (float)0.1;
            points[3].Y = (float)0.3;
            points[4].X = (float)0.2;
            points[4].Y = (float)0.2;
            points[5].X = (float)0.5;
            points[5].Y = (float)0.5;
            points[6].X = (float)0.7;
            points[6].Y = (float)0.5;
            points[7].X = (float)0.7;
            points[7].Y = (float)0.7;
            points[8].X = (float)0.5;
            points[8].Y = (float)0.7;
            points[9].X = (float)0.6;
            points[9].Y = (float)0.6;

            for (int k = 0; k < num; k++)
                iscenter[k] = 0;
        }
   /// <summary>
        /// 初始化消息
        /// </summary>
        private void InitMsga()
        {
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    msga[i, j] = 0;
                    msgr[i, j] = 0;
                    oldmsga[i, j] = 0;
                    oldmsgr[i, j] = 0;
                }
            }
        }
        
         /// <summary>
        /// 计算欧式距离的相反数
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double CalDistant(PointF p1, PointF p2)
        {
            return -(p1.X - p2.X) * (p1.X - p2.X) - (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }
         /// <summary>
        /// 创建相似度矩阵
        /// </summary>
        private void CreateSimilarMatrix()
        {
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    similarmatrix[i, j] = CalDistant(points[i], points[j]);
                }
            }
            double median = ReturnMedian(similarmatrix);
            pk = median / 2;                                         //取中值作为preference的值
            for (int i = 0; i < num; i++)
            {
                similarmatrix[i, i] = pk;
            }
        }
         /// <summary>
        /// 更新消息
        /// </summary>
        private void UpdateMsg()
        {
            //更新msgr.R(i,k)=S(i,k)- max{A(i,j)+S(i,j)}(j {1,2,……,N,但j≠k})
            for (int i = 0; i < num; i++)
            {
                for (int k = 0; k < num; k++)
                {
                    //首先得到max{A(i,j)+S(i,j)}(j!=k)
                    double max_tmp;
                    //赋初值
                    if (k == 0)
                    {
                        max_tmp = msga[i, 1] + similarmatrix[i, 1];
                    }
                    else
                    {
                        max_tmp = msga[i, 0] + similarmatrix[i, 0];
                    }
                    for (int j = 0; j < num; j++)
                    {
                        if (j != k)
                        {
                            if (max_tmp < (msga[i, j] + similarmatrix[i, j]))
                            {
                                max_tmp = msga[i, j] + similarmatrix[i, j];
                            }
                        }
                    } //end get the max

                    msgr[i, k] = similarmatrix[i, k] - max_tmp;
                }
            }
            //更新msgr[k,k].R(k,k)=P(k)-max{A(k,j)+S(k,j)} (j {1,2,……,N,但j≠k})
            for (int i = 0; i < num; i++)
            {
                double max_tmp;
                //赋初值
                if (i == 0)
                {
                    max_tmp = msga[i, 1] + similarmatrix[i, 1];
                }
                else
                {
                    max_tmp = msga[i, 0] + similarmatrix[i, 0];
                }
                for (int j = 0; j < num; j++)
                {
                    if (j != i)
                    {
                        if ((msga[i, j] + similarmatrix[i, j]) > max_tmp)
                        {
                            max_tmp = msga[i, j] + similarmatrix[i, j];
                        }
                    }
                }
                msgr[i, i] = pk - max_tmp;
            }
            //为容易看先分开写
            //更新msga.A(i,k)=min{0,R(k,k)+  (j {1,2,……,N,但j≠i且j≠k})
            for (int i = 0; i < num; i++)
            {
                for (int k = 0; k < num; k++)
                {
                    //求得max部分
                    double sum_tmp = 0;
                    for (int j = 0; j < num; j++)
                    {
                        if (j != i)
                        {
                            if (msgr[j, k] > 0)
                            {
                                sum_tmp += msgr[j, k];
                            }
                        }
                    } //end 求max部分
                    double addtmp = msgr[k, k] + sum_tmp;
                    if (addtmp < 0)
                    {
                        msga[i, k] = addtmp;
                    }
                    else
                    {
                        msga[i, k] = 0;
                    }
                }
            }//end 更新msga
        } //end updatemsg
         /// <summary>
        /// 保存到旧消息数组
        /// </summary>
        private void GetOldMsg()
        {
            for (int i = 0; i < num; i++)
            {
                for (int k = 0; k < num; k++)
                {
                    oldmsga[i, k] = msga[i, k];
                    oldmsgr[i, k] = msgr[i, k];
                }
            }
        }
}
