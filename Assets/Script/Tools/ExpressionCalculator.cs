using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionCalculator
{
    //PriorityJudge[i][j]��ʾ��ջ������Ϊi����ǰɨ�����Ϊj
    //true��ʾi�����ȼ��ߣ�����ջ����i�����й���RPN��false��ʾj�����ȼ��ߣ���j��ջ
    private static bool[][] PriorityJudge = new bool[128][];
    private static char[] Operator = { '+', '-', '*', '/', '(', ')' };
    Dictionary<int, int> nums = new Dictionary<int, int>();

    public ExpressionCalculator()
    {
        for (int i = 0; i < 128; i++)
        {
            PriorityJudge[i] = new bool[128];
        }
        //��ֵ��false��ֻ��j��ջ�������Ҫд��true
        for (int i = 0; i < 5; i++)
        {
            //����ջΪ��ʱ���κη��Ŷ���ջ
            PriorityJudge['#'][Operator[i]] = true;
            //ջ��������'('ʱ���κη��Ŷ���ջ
            PriorityJudge['('][Operator[i]] = true;
            PriorityJudge[Operator[i]]['('] = true;
        }
        PriorityJudge['+']['*'] = true;
        PriorityJudge['+']['/'] = true;
        PriorityJudge['-']['*'] = true;
        PriorityJudge['-']['/'] = true;
    }
    private bool IsOperator(char c)
    {
        if (c >= '0' && c <= '9') return false;
        else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')') return true;
        else throw new Exception("���ʽ�Ƿ�");
    }
    private List<string> GenerateRPN(string str)
    {
        List<string> RPN = new List<string>();
        Stack<char> symbol = new Stack<char>();
        symbol.Push('#');
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (IsOperator(c))
            {
                if (c == ')')
                {
                    do
                    {
                        RPN.Add(symbol.Pop().ToString());
                    } while (symbol.Peek() != '(');
                    symbol.Pop();
                }
                else
                {
                    if (PriorityJudge[symbol.Peek()][c])
                    {
                        symbol.Push(c);
                    }
                    else
                    {
                        RPN.Add(symbol.Pop().ToString());
                        i--;//��������һ���Ƚ�
                    }
                }
            }
            else
            {
                string temp = "";
                while (i < str.Length && !IsOperator(str[i]))
                {
                    temp += str[i];
                    i++;
                }
                i--;

                if (!nums.ContainsKey(int.Parse(temp)))
                {
                    throw new Exception("��ʹ�ø�������");
                }
                else if (--nums[int.Parse(temp)] == -1)
                {
                    throw new Exception("ÿ������ֻ����ʹ��һ��");
                }
                RPN.Add(temp);
            }
        }
        while (symbol.Peek() != '#')
        {
            RPN.Add(symbol.Pop().ToString());
        }
        return RPN;
    }
    private int calc(int a, int b, char c)
    {
        switch (c)
        {
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                return a / b;
            default:
                return 0;
        }
    }

    public int calc(string expression, int[] selectNums)
    {
        nums.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (nums.ContainsKey(selectNums[i]))
            {
                nums[selectNums[i]]++;
            }
            else
            {
                nums.Add(selectNums[i], 1);
            }

        }

        List<string> RPN = GenerateRPN(expression);
        Stack<int> num = new Stack<int>();
        for (int i = 0; i < RPN.Count; i++)
        {
            string s = RPN[i];
            if (IsOperator(s[0]))
            {
                int a = num.Pop();
                int b = num.Pop();
                int res = calc(b, a, s[0]);
                num.Push(res);
            }
            else
            {
                num.Push(int.Parse(s));
            }
        }
        return num.Peek();
    }


    //public bool judgeExpressionLegality(string expression )
    //{

    //    //expression.Split
    //    //nums.Find()
    //    return false;
    //}

}

