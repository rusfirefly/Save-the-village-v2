using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Practcal5_7 : MonoBehaviour
{
    
    [SerializeField] Text TextResult;

    [Header("Задание 1\nСумма чётных чисел заданного диапазона")]
    [SerializeField] int firstNumber = 7;
    [SerializeField] int secondNumber = 21;
    [SerializeField] bool viewOnluReuslt = true;

    [Header("Задание 2\n Сумма чётных чисел в заданном массиве")]
    [SerializeField] int[] array_task2 = { 81, 22, 13, 54, 10, 34, 15, 26, 71, 68 };//значения по умолчанию

    [Header("Задание 3\nИндекс первого вхождения числа в массив")]
    [SerializeField] int number = 34;
    [SerializeField] int[] array_task3 = {81, 22, 13, 34, 10, 34, 15, 26, 71, 68};


    [Header("Задание 4\nСортировка выбором")]
    [SerializeField] int[] array_task4 = { 81, 22, 13, 34, 10, 34, 15, 26, 71, 68 };

    public void Task1()
    {
        //Сумма чётных чисел заданного диапазона.
        int a = firstNumber,
            b = secondNumber;

        int[] arr = new int[a+b];
        string str = "";
        string number_even = "";
        int sum = 0;
  
        for (int i = a; i <= b; i++)
        {
            arr[i] = i;
            if (arr[i] % 2 == 0)
            {
                sum += arr[i];
                number_even += $"{arr[i]} ";//для вывода четных на экран
            }
            str += $"{arr[i]} ";//весь массив с числами
        }
        if(!viewOnluReuslt)
            TextResult.text = $"Данные:\n{str}\nОт {a} До {b}\nВсе чётные числа:\n{number_even}\nСумма чётных чисел заданного диапазона Result = {sum}";
        else TextResult.text = $"Сумма чётных чисел заданного диапазона Result = {sum}";
    }

    public void Task2()
    {
        //Сумма чётных чисел в заданном массиве
        int sum = 0;
        string arr_str = "";
        string number_even = "";

        for (int i=0;i< array_task2.Length;i++)
        {
            if(array_task2[i]%2==0)
            {
                sum += array_task2[i];
                number_even += $"{array_task2[i]} ";
                
            }
            arr_str += $"{array_task2[i]} ";
        }
        TextResult.text = $"Данные:\n{arr_str}\nВсе чётные числа:\n{number_even}\nСумма чётных чисел в заданном массиве Result = {sum}";
    }

    public void Task3()
    {
        int index = 0;
        
        string str = "";
        int i = 0;
        for (i=0;i< array_task3.Length;i++)
            str += $"{array_task3[i]} ";

        i = 0;
        foreach (int number_in_array in array_task3)
        {
            
            if (number_in_array == number)
            {
                index = i;
                break;
            }
            else
                index = -1;

            i++;
        }
        TextResult.text = $"Массив:\n{str}\nИндекс первого вхождения числа {number} в массив: {index}";
    }

    public void Task4()
    {
        //сортировка
        string array_sort = "";
        string array_no_sort = "";
        int[] arr = (int[])array_task4.Clone();
        int min;

        foreach (int a in array_task4)
            array_no_sort += $"{a} ";

        for (int i = 0;i< arr.Length;i++)
        {
            
            min = i;//начинаем с 0 эл-та 

            for(int j = i+1 ; j< arr.Length; j++)//проходим по всему массиву и ищем минимальное
            {
                if (arr[j] < arr[min])//сравниваем
                    min = j;//ооо, найдем минимальный, записываем в min индекс(т.е. позицию найденного числа)

            }

            //здесь меняем местами два значения
            int temp = arr[i];
            arr[i] = arr[min];
            arr[min] = temp;

            array_sort += $"{arr[i]} ";//для вывода на экран отсортированного массива
        }


        TextResult.text = $"Оригинальный массив:\n{array_no_sort}\nОтсортированный массив:\n{array_sort}" ;

    }


}
