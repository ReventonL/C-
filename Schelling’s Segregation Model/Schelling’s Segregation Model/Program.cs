using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schelling_s_Segregation_Model
{

    class Program
    {
        public static int[] Maps;//用静态字段模拟地图
        public static int Ticks = 0;//迭代次数
        public static int N;//正方形边长
        public static double SimilarWanted;//相似度阈值
        public static ArrayList MapsNoOne = new ArrayList();//无人居住的地点
        public static Dictionary<int, int> MapsNeedMove = new Dictionary<int, int>();//需要搬家的点
        public static double Total;//总人口
        public static bool judge = true;//用于判断是否迭代完成
        public static double totalUnHappy;//不满意住户总数
        public static double similarNeighbors;//相似邻居总数
        public static double totalNeighbors;//总邻居数

        static void Main(string[] args)
        {
            while (true)
            {
                Ticks = 0;             //初始化
                totalUnHappy = 0;
                similarNeighbors = 0;
                totalNeighbors = 0;
                Total = 0;
                Console.Clear();
                Console.Write("请输入矩阵长度N:");
                string strN = Console.ReadLine();
                N = Convert.ToInt32(strN);//矩阵长度N
                Maps = new int[N * N];
                Console.Write("请输入相似度阈值（范围0-1，例如0.3）:");
                string strSimilarWanted = Console.ReadLine();
                SimilarWanted = Convert.ToDouble(strSimilarWanted);//相似度阈值
                InitMaps();//初始化地图
                Total = Maps.Length - MapsNoOne.Count;//总人口数=最大人口容量-无人居住的地点
                for (int i = 0; i < N * N; i++)//计算不满意的住户总数
                {
                    if (!HappyorNot(i))
                        totalUnHappy++;
                }
                DrawMaps();//画地图
                while (true)
                {
                    DrawHead();
                    string strgo = Console.ReadLine();
                    if (strgo == "1")//输入1，表示执行一次迭代
                    {
                        similarNeighbors = 0;//相似的住户总数
                        totalNeighbors = 0;//总邻居数
                        totalUnHappy = 0;//不满意住户总数
                        judge = false;
                        for (int i = 0; i < N * N; i++)
                        {
                            if (!HappyorNot(i))
                            {
                                totalUnHappy++;
                                MapsNeedMove.Add(i, Maps[i]);   //记录不高兴住户的坐标以及肤色
                                judge = true;
                            }
                        }

                        foreach (KeyValuePair<int, int> kv in MapsNeedMove)//遍历键值对集合
                        {
                            Maps[kv.Key] = 1;                   //将不满意的住户搬离该点（即相应坐标对应的值置1）
                            MapsNoOne.Add(kv.Key);
                        }

                        for (int i = 0; i < N * N; i++)        //为不满意的住户随机搬家
                        {
                            if (MapsNeedMove.ContainsKey(i))
                            {
                                Move(i);//搬家函数
                            }
                        }

                        if (!judge) //迭代完成
                        {
                            Console.WriteLine("已经没有不高兴的人！最终迭代完成！总迭代次数:{0}", Ticks);
                        }
                        if (judge)//迭代未完成
                        {
                            Ticks++;
                            Console.Clear();
                            DrawMaps();
                        }
                    }
                    else if (strgo == "2")
                    {
                        while (judge)//当迭代未完成时，执行
                        {
                            similarNeighbors = 0;
                            totalNeighbors = 0;
                            totalUnHappy = 0;
                            judge = false;
                            for (int i = 0; i < N * N; i++)
                            {
                                if (!HappyorNot(i))
                                {
                                    totalUnHappy++;
                                    MapsNeedMove.Add(i, Maps[i]);   //记录不高兴住户的坐标以及肤色
                                    judge = true;
                                }
                            }

                            foreach (KeyValuePair<int, int> kv in MapsNeedMove)
                            {
                                Maps[kv.Key] = 1;
                                MapsNoOne.Add(kv.Key);
                            }

                            for (int i = 0; i < N * N; i++)
                            {
                                if (MapsNeedMove.ContainsKey(i))
                                {
                                    Move(i);
                                }
                            }

                            if (!judge)
                            {
                                Console.Clear();
                                DrawMaps();
                                Console.WriteLine("已经没有不高兴的人！最终迭代完成！总迭代次数:{0}", Ticks);
                            }
                            if (judge)
                            {
                                Ticks++;                                
                            }
                        }
                    }
                    else if (strgo == "0")
                        break;
                }
            }
        }
        public static void DrawHead()
        {
            Console.Write("请输入数字，1表示执行一次仿真，2表示执行到所有人都满意，0表示初始化:");
        }
        /// <summary>
        /// 初始化地图
        /// </summary>
        public static void InitMaps()
        {
            Random r = new Random();
            int rNum = r.Next(1, 5);
            int rNumber;
            for (int i = 0; i < N * N; i++)//随机给地图数组赋值，值为1代表无人居住，2代表红色，3代表绿色
            {
                rNum = r.Next(1, 5);//用于设定人口密度
                switch (rNum)
                {
                    case 1:
                    case 2:
                    case 3:
                        rNumber = r.Next(1, 4);
                        break;
                    default:
                        rNumber = r.Next(2, 4);
                        break;
                }
                Maps[i] = rNumber;
            }
            MapsNoOne.Clear();
            for (int i = 0; i < N * N; i++)//将无人居住的地点放在集合内保存
            {
                if (Maps[i] == 1)
                {
                    if (!MapsNoOne.Contains(i))
                        MapsNoOne.Add(i);
                }
            }
        }
        /// <summary>
        /// 画地图
        /// </summary>
        public static void DrawMaps()
        {
            Console.WriteLine("相似度阈值:{0}%,总人口数:{1},迭代次数:{2}次", SimilarWanted * 100, Total, Ticks);
            Console.WriteLine("相似度:{0:0.00}%,不高兴人口比例:{1:0.00}%", similarNeighbors / totalNeighbors * 100, totalUnHappy / Total * 100);
            for (int i = 0; i < N + 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("★");
            }
            Console.WriteLine();
            for (int i = 0; i < N; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("★");
                for (int j = 0; j < N; j++)
                {
                    Console.Write(DrawBlocks(N * i + j));
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("★");
            }
            for (int i = 0; i < N + 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("★");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="i">地图的坐标</param>
        /// <returns>对应的地图</returns>
        public static string DrawBlocks(int i)
        {
            string str = "";
            switch (Maps[i])
            {
                case 1://表示无人居住
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 2://红色、绿色表示不同人种
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            str = "■";
            return str;
        }
        /// <summary>
        /// 判断住户是否高兴
        /// </summary>
        /// <param name="i">地图坐标</param>
        /// <returns>true表示高兴，false表示不高兴</returns>
        public static bool HappyorNot(int i)
        {
            bool b = true;
            double count = 0;
            double count1 = 0;
            if (Maps[i] == 1)//该点如果为空，则不需要搬家
            {
                return b;
            }
            else if (i == 0)//左上角顶点搬家判断
            {
                if (Maps[i + 1] != 1)
                {
                    count1++;//如果邻居的点不为空，则该点邻居总住户数+1
                    if (Maps[i] == Maps[i + 1])
                        count++;//如果邻居的肤色与自己相同，则满意的邻居数+1
                }
                if (Maps[i + N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N])
                        count++;
                }
                if (Maps[i + N + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N + 1])
                        count++;
                }
            }
            else if (i == N - 1)//右上角顶点判断
            {
                if (Maps[i - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - 1])
                        count++;
                }
                if (Maps[i + N - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N - 1])
                        count++;
                }
                if (Maps[i + N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N])
                        count++;
                }
            }
            else if (i == N * (N - 1))//左下角顶点判断
            {
                if (Maps[i + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + 1])
                        count++;
                }
                if (Maps[i - (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N - 1)])
                        count++;
                }
                if (Maps[i - N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - N])
                        count++;
                }
            }
            else if (i == N * N - 1)//右下角顶点判断
            {
                if (Maps[i - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - 1])
                        count++;
                }
                if (Maps[i - N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - N])
                        count++;
                }
                if (Maps[i - (N + 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N + 1)])
                        count++;
                }
            }
            else if (i >= 1 && i <= N - 2)//上边判断
            {
                if (Maps[i + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + 1])
                        count++;
                }
                if (Maps[i - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - 1])
                        count++;
                }
                if (Maps[i + (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + (N - 1)])
                        count++;
                }
                if (Maps[i + N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N])
                        count++;
                }
                if (Maps[i + N + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N + 1])
                        count++;
                }
            }
            else if (i >= N * (N - 1) + 1 && i <= N * N - 2)//底边判断
            {
                if (Maps[i + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + 1])
                        count++;
                }
                if (Maps[i - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - 1])
                        count++;
                }
                if (Maps[i - (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N - 1)])
                        count++;
                }
                if (Maps[i - N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - N])
                        count++;
                }
                if (Maps[i - (N + 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N + 1)])
                        count++;
                }
            }
            else if (i % N == 0 && i != 0 && i != N * (N - 1) - 2)//左边判断
            {
                if (Maps[i + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + 1])
                        count++;
                }
                if (Maps[i - (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N - 1)])
                        count++;
                }
                if (Maps[i + N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N])
                        count++;
                }
                if (Maps[i - N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - N])
                        count++;
                }
                if (Maps[i + (N + 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + (N + 1)])
                        count++;
                }
            }
            else if (i % N == N - 1 && i != N - 1 && i != N * N - 1)//右边判断
            {
                if (Maps[i - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - 1])
                        count++;
                }
                if (Maps[i + (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + (N - 1)])
                        count++;
                }
                if (Maps[i + N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N])
                        count++;
                }
                if (Maps[i - N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - N])
                        count++;
                }
                if (Maps[i - (N + 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N + 1)])
                        count++;
                }
            }
            else//中间的住户判断
            {
                if (Maps[i + 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + 1])
                        count++;
                }
                if (Maps[i - 1] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - 1])
                        count++;
                }
                if (Maps[i + (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + (N - 1)])
                        count++;
                }
                if (Maps[i - (N - 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N - 1)])
                        count++;
                }
                if (Maps[i + N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + N])
                        count++;
                }
                if (Maps[i - N] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - N])
                        count++;
                }
                if (Maps[i + (N + 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i + (N + 1)])
                        count++;
                }
                if (Maps[i - (N + 1)] != 1)
                {
                    count1++;
                    if (Maps[i] == Maps[i - (N + 1)])
                        count++;
                }
            }
            if (count1 != 0)//当邻居数不为0时，才计算满意度
            {
                totalNeighbors += count1;//用于计算总邻居数
                similarNeighbors += count;//用于计算相似邻居数
                if ((count / count1) - SimilarWanted >= 0)//当满意度大于满意度阈值时，不搬家，否则搬家
                    b = true;
                else
                    b = false;
            }
            return b;
        }
        /// <summary>
        /// 搬家
        /// </summary>
        /// <param name="i">原地址</param>
        public static void Move(int i)
        {
            Random r = new Random();
            int rNum = 0;
            rNum = r.Next(0, Maps.Length);//随机搬家
            while (Maps[rNum] != 1)
            {
                rNum = r.Next(0, Maps.Length);//若该点不为空则继续寻找搬家地点
            }
            Maps[rNum] = MapsNeedMove[i];//搬家成功
            MapsNoOne.Remove(rNum);
            MapsNeedMove.Remove(i);//将搬家成功的点从需要搬家的名单中删除
        }
    }
}

