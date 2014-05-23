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
}
