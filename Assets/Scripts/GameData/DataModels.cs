using System;
using System.Collections.Generic;

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
        public List<int> id_area;
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

    [Serializable]
    public class Translations
    {
        public AccessibilityData accessibility;
        public ButtonsData buttons;
        public PlaceholdersData placeholders;
        public MessagesData messages;
        public HeadingsData headings;
        public AnimalListData animal_list_scene;
        public AnimalProfileData animal_profile_scene;
        public ScoreData score_scene;
        public InfoData info_scene;
        public InstructionData instruction_scene;
        public PrivacyData privacy_scene;
        public ProfileData profile_scene;
        public QuizData quiz_scene;
        public WelcomeData welcome_scene;

        public class AccessibilityData
        {
            public string font_size;
            public string dyslexia;
            public string contrast_text;
            public string tts_text;
            public string small;
            public string medium;
            public string large;
        }

        public class ButtonsData
        {
            public string btn_confirm;
            public string btn_end_instruction;
            public string btn_game;
            public string btn_globus_mode;
            public string btn_hologram;
            public string btn_home;
            public string btn_learn;
            public string btn_list_mode;
            public string btn_login;
            public string btn_new_quiz;
            public string btn_next_question;
            public string btn_ok;
            public string btn_quiz;
            public string btn_register;
            public string btn_save;
            public string btn_see_results;
            public string btn_start_quiz;
            public string btn_start;
        }

        public class PlaceholdersData
        {
            public string placeholder_new_password;
            public string placeholder_password_repeat;
            public string placeholder_password;
            public string placeholder_repeat_new_password;
            public string placeholder_username;
        }

        public class MessagesData
        {
            public string msg_check_internet;
            public string msg_login_success;
            public string msg_password_short;
            public string msg_password_wrong;
            public string msg_passwords_mismatch;
            public string msg_register_success;
            public string msg_username_short;
            public string msg_username_wrong;
        }

        public class HeadingsData
        {
            public string heading_choose_device;
            public string heading_learn_method;
            public string heading_login;
            public string heading_register;
        }

        public class AnimalListData
        {
            public string list_text;
        }

        public class AnimalProfileData
        {
            public string threed_text;
            public string endangerment_status;
            public string region;
            public string habitat;
            public string weight;
            public string diet;
            public string population;
        }

        public class ScoreData
        {
            public string try_again;
            public string bravo;
            public string unlocked_animals;
        }

        public class InfoData
        {
            public string developed_by;
            public string univ_text;
        }

        public class InstructionData
        {
            public string instruction_one_heading;
            public string instruction_one_materials;
            public string instruction_one_supply;
            public string instruction_two;
            public string instruction_three;
        }

        public class PrivacyData
        {
            public string header;
            public string body;
        }

        public class ProfileData
        {
            public string level;
            public string experience;
        }

        public class QuizData
        {
            public string choose_difficulty;
            public string easy;
            public string medium;
            public string hard;
        }

        public class WelcomeData
        {
            public string hello_text;
            public string welcome;
        }
    }
}