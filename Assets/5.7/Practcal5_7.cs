using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Practcal5_7 : MonoBehaviour
{
    
    [SerializeField] Text TextResult;

    [Header("������� 1\n����� ������ ����� ��������� ���������")]
    [SerializeField] int firstNumber = 7;
    [SerializeField] int secondNumber = 21;
    [SerializeField] bool viewOnluReuslt = true;

    [Header("������� 2\n ����� ������ ����� � �������� �������")]
    [SerializeField] int[] array_task2 = { 81, 22, 13, 54, 10, 34, 15, 26, 71, 68 };//�������� �� ���������

    [Header("������� 3\n������ ������� ��������� ����� � ������")]
    [SerializeField] int number = 34;
    [SerializeField] int[] array_task3 = {81, 22, 13, 34, 10, 34, 15, 26, 71, 68};


    [Header("������� 4\n���������� �������")]
    [SerializeField] int[] array_task4 = { 81, 22, 13, 34, 10, 34, 15, 26, 71, 68 };

    public void Task1()
    {
        //����� ������ ����� ��������� ���������.
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
                number_even += $"{arr[i]} ";//��� ������ ������ �� �����
            }
            str += $"{arr[i]} ";//���� ������ � �������
        }
        if(!viewOnluReuslt)
            TextResult.text = $"������:\n{str}\n�� {a} �� {b}\n��� ������ �����:\n{number_even}\n����� ������ ����� ��������� ��������� Result = {sum}";
        else TextResult.text = $"����� ������ ����� ��������� ��������� Result = {sum}";
    }

    public void Task2()
    {
        //����� ������ ����� � �������� �������
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
        TextResult.text = $"������:\n{arr_str}\n��� ������ �����:\n{number_even}\n����� ������ ����� � �������� ������� Result = {sum}";
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
        TextResult.text = $"������:\n{str}\n������ ������� ��������� ����� {number} � ������: {index}";
    }

    public void Task4()
    {
        //����������
        string array_sort = "";
        string array_no_sort = "";
        int[] arr = (int[])array_task4.Clone();
        int min;

        foreach (int a in array_task4)
            array_no_sort += $"{a} ";

        for (int i = 0;i< arr.Length;i++)
        {
            
            min = i;//�������� � 0 ��-�� 

            for(int j = i+1 ; j< arr.Length; j++)//�������� �� ����� ������� � ���� �����������
            {
                if (arr[j] < arr[min])//����������
                    min = j;//���, ������ �����������, ���������� � min ������(�.�. ������� ���������� �����)

            }

            //����� ������ ������� ��� ��������
            int temp = arr[i];
            arr[i] = arr[min];
            arr[min] = temp;

            array_sort += $"{arr[i]} ";//��� ������ �� ����� ���������������� �������
        }


        TextResult.text = $"������������ ������:\n{array_no_sort}\n��������������� ������:\n{array_sort}" ;

    }


}
