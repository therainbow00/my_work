﻿using Numbers;
using System;
using System.Collections.Generic;
using static System.Console;

namespace Numbers
{
    internal class NumberStuff
    {
        internal static void Main(string[] args)
        {
            Random rand = new Random();
            Nums nums = new Nums();
            NumFunctions numFunctions = new NumFunctions();
            Write("Number of loops: ");
            string? input = ReadLine();
            int num = int.Parse(input);
            Total total;

            for (int i = 1; i <= num; i++)
            {
                total = new(rand.Next(101));
                SetNumbersInt(rand, numFunctions);
                SetNumbersDecimal(rand, numFunctions);
                PrintStuff(numFunctions, total, rand, i);
            }
            WriteLine(nums.Print());
        }

        static void SetNumbersInt(Random rand, NumFunctions numFunctions)
        {
            int length;
            length = rand.Next(100);
            numFunctions.SetNum1Or3(length);
            length = rand.Next(100);
            numFunctions.SetNum2Or4(length);
        }

        static void SetNumbersDecimal(Random rand, NumFunctions numFunctions)
        {
            double length;
            length = rand.Next(100);
            numFunctions.SetNum1Or3(length + .75);
            length = rand.Next(100);
            numFunctions.SetNum2Or4(length + .56);
        }

        static void PrintStuff(NumFunctions numFunctions, Total total, Random rand, int index)
        {
            for (int l = 1; l <= 33; l++) Write("=");
            Write($" {index} ");

            if (index > 9) for (int l = 1; l <= 33; l++) Write("=");
            else for (int l = 1; l <= 34; l++) Write("=");

            WriteLine();

            WriteLine($"The greater number is: {numFunctions.Greater(numFunctions.Num1, numFunctions.Num2)} (Number 1 was: {numFunctions.Num1} | Number 2 was: {numFunctions.Num2})");

            WriteLine("Total: " + total.TotalNumber);
            WriteLine($"The greater number is: {numFunctions.Greater(numFunctions.Num3, numFunctions.Num4).ToString("0")} (Number 3 was: {numFunctions.Num3.ToString("0")} | Number 4 was: {numFunctions.Num4.ToString("0")})");

            total = new Total(rand.Next(101));
            WriteLine("Total: " + total.TotalNumber);

            for (int l = 1; l <= 70; l++) Write("=");

            WriteLine();
        }
    }

    internal class Nums : Done
    {
        protected int num1;
        protected int num2;

        protected double num3;
        protected double num4;

        public override string Print() { return "Done"; }
    }

    internal class Total : Nums
    {
        private int totalNumber;

        public Total(int num) { TotalNumber = (PrintNums(num)); }

        public int TotalNumber
        {
            get { return totalNumber; }
            set { totalNumber = value; }
        }

        private int PrintNums(int length)
        {
            int temp = 0;
            for (int i = 0; i < length; i++) temp += i;
            return temp;
        }
    }

    internal class NumFunctions : Nums
    {
        public int Num1
        {
            get { return num1; }
            set { num1 = value; }
        }
        public int Num2
        {
            get { return num2; }
            set { num2 = value; }
        }
        public double Num3
        {
            get { return num3; }
            set { num3 = value; }
        }
        public double Num4
        {
            get { return num4; }
            set { num4 = value; }
        }

        public void SetNum1Or3(int num) { Num1 = num; }
        public void SetNum1Or3(double num) { Num3 = num; }

        public void SetNum2Or4(int num) { Num2 = num; }
        public void SetNum2Or4(double num) { Num4 = num; }

        public int Greater(int num1, int num2) { if (num1 < num2) return num2; return num1; }
        public double Greater(double num1, double num2) { if (num1 < num2) return num2; return num1; }
    }


    internal abstract class Done { public abstract string Print(); }
}