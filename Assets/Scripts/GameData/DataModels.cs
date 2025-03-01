using System;

namespace HoloZoo.DataModels
{
    [Serializable]
    public class Area
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class Animal
    {
        public int id;
        public int id_area;
        public int level;
        public string url_model;
        public string url_slika;
        public string weight;
        public int population;
        public string name;
        public string endangerment_status;
        public string habitat;
        public string diet;
    }
    [Serializable]
    public class Question
    {
        public int id;
        public int id_animal;
        public int correct_answer;
        public string question_text;
        public string answer_one;
        public string answer_two;
        public string answer_three;
    }

    [Serializable]
    public class User
    {
        public int id;
        public int level;
        public string username;
        public string password;
        public int experience;
    }
}