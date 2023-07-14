using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WordFamilies
{
    internal class Program
    {
        static readonly string textFile = @"C:\Users\mayro\OneDrive\Escritorio\Git\Cheater_Hangman\dictionary.txt";
        public static int random = 0;

        public static List<string> dictionary = new List<string>();


        public static List<string> positions = new List<string>(); //char position in the word
        public static List<int> count = new List<int>(); //count of char
        public static List<int> combinations = new List<int>(); //possible numbers obtained with count (see snipping tool)
        public static List<int> comb_count = new List<int>(); //count of combinations
        public static List<string> possible_words = new List<string>();
        public static List<string> possible_positions = new List<string>();
        public static List<string> resulted_words = new List<string>();



        static void Main(string[] args)
        {


            //Game presentation and Difficulty selection
            Console.WriteLine("WELCOME TO GUESS THE WORD!!");
            Console.WriteLine("---------------------------");

            Console.WriteLine(" ");
            Console.WriteLine("Choose the game difficulty: \n" +
                "1. Easy \n" +
                "2. Diffcult \n");
            string difficulty = Console.ReadLine();


            string secret_word = Word(); //Secret word
            //Console.WriteLine(secret_word);

            int lifes = secret_word.Length * 2;//Gets the numberof lifes, twice the secret word letters
            string secret = secret_word;

            //Hiddes secret word
            for (int i = 0; i < secret.Length; i++)
            {

                secret = secret.Replace(secret[i], '-');
            }
            secret_word = secret_word.ToLower();

            List<char> letters_used = new List<char>();//Get a list of the letters used


            StringBuilder tape = new StringBuilder();
            tape.Append(secret);

            //Three states: START, NEXT, HALT
            //Game starts
            switch (difficulty)
            {
                case "1":
                    Console.WriteLine("Easy mode selected");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    GameEasy(secret_word, lifes, tape, letters_used);
                    break;
                case "2":
                    Console.WriteLine("Difficult mode selected");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    GameDifficult(secret_word, lifes, tape, letters_used);
                    break;

            }

        }

        public static void Dictionary(List<string> dictionary, string choice)
        {
            Random rd = new Random();

            //Clear all lists for a new round
            count.Clear();
            resulted_words.Clear();
            positions.Clear(); //char position in the word
            combinations.Clear(); //possible numbers obtained with count
            comb_count.Clear(); //count of combinations
            possible_words.Clear();
            possible_positions.Clear();
            resulted_words.Clear();

            //Go through all words in the dictionary list
            for (int i = 0; i < dictionary.Count; i++)
            {
                //Temporal variable to count the number of combinations
                int count_tmp = 0;
                //Initialise the position of the letters in the current word
                positions.Add("");

                //for char c in word i...
                for (int c = 0; c < dictionary[i].Length; c++)
                {
                    //... if is the same as the letter chosen by the user
                    if (dictionary[i][c] == choice[0])
                    {
                        count_tmp++;//Increase temporal counter
                        positions[i] = positions[i] + c + ","; //Save position for the current char
                    }

                }
                //Add to the official counter the current temporal one
                count.Add(count_tmp);
                

            }

            for (int i = 0; i < dictionary.Count; i++)
            {
                if (i == 0)
                {
                    //Add to the combinations list first type if combination
                    combinations.Add(count[i]);

                    comb_count.Add(1);
                }
                else
                {
                    bool exist = false;

                    //Check if the combination already exists
                    for (int a = 0; a < combinations.Count; a++)
                    {
                        if (combinations[a] == count[i])
                        {
                            exist = true;
                            comb_count[a]++; //Increase count of current combination
                        }

                    }

                    //If combination doesnt exist, add it to the counter of combinations
                    if (exist == false)
                    {
                        combinations.Add(count[i]);
                        comb_count.Add(1);
                    }

                }


            }

            
            int max_count = 0;
            int max_coincidence = 0;

            //Find the most common combination and its highest count
            for (int i = 0; i < combinations.Count; i++)
            {
                if (comb_count[i] > max_count)
                {
                    max_count = comb_count[i];
                    max_coincidence = combinations[i];
                }

            }

            //Select the words with that combination
            for (int i = 0; i < count.Count; i++)
            {
                if (count[i] == max_coincidence)
                {
                    possible_words.Add(dictionary[i]);
                    possible_positions.Add(positions[i]);
                }
            }

            //Choose a random word from that combination
            random = rd.Next(0, possible_words.Count);

            //Find and select the words with the letters in the same position 
            for (int i = 0; i < possible_words.Count; i++)
            {
                if (possible_positions[i] == possible_positions[random])
                {
                    resulted_words.Add(possible_words[i]);
                }
            }

            //Clear the dictionary and update it with the new words
            dictionary.Clear();

            for (int i = 0; i < resulted_words.Count; i++)
            {
                dictionary.Add(resulted_words[i]);
            }




        }

        public static void Dictionary2(List<string> dictionary, string choice)
        {
            List<string> result = new List<string>();

            //Loop through the dictionary ooking for the words with less times the choice
            for (int i = 0; i < dictionary.Count; i++)
            {
                int c_count = 0;
                for (int c = 0; c < dictionary[i].Length; c++)
                {
                    if (dictionary[i][c] == choice[0])
                    {
                        c_count++;
                    }
                }
                if (c_count == 0)
                {
                    result.Add(dictionary[i]);
                    //Console.WriteLine(dictionary[i]);
                }

            }

            int letter_count = 0; //Checks how many letter we have already checked

            //if no words with 0 times choice, looks for the group with less times that words
            while (result.Count == 0)
            {
                letter_count++;
                for (int i = 0; i < dictionary.Count; i++)
                {
                    int c_count = 0;
                    for (int c = 0; c < dictionary[i].Length; c++)
                    {
                        if (dictionary[i][c] == choice[0])
                        {
                            c_count++;
                        }
                    }
                    if (c_count == letter_count)
                    {
                        result.Add(dictionary[i]);
                        //Console.WriteLine(dictionary[i]);
                    }

                }
            }
            //with more letter than 0

            if (letter_count > 0 && result.Count > 1)
            {

                List<string> result_tmp = new List<string>();
                List<string> positions_tmp = new List<string>(); //Count positions of the letter in the word
                List<int> quantity_tmp = new List<int>();//Counts how many words are with the same position
                List<string> words_tmp = new List<string>();//Looks for the words with that letter position

                //finds letter positions
                for (int i = 0; i < result.Count; i++)
                {
                    string positions = "";
                    for (int c = 0; c < result[i].Length; c++)
                    {
                        if (result[i][c] == choice[0])
                        {
                            positions = positions + c + ",";
                        }
                    }

                    if (i == 0) //If first words, adds to list result_tmp
                    {
                        result_tmp.Add(positions);
                        quantity_tmp.Add(1);
                        words_tmp.Add(positions);
                    }

                    else//if not the first, checks if the combination exists
                    {
                        bool exist = false;
                        for (int a = 0; a < result_tmp.Count; a++)
                        {
                            if (result_tmp[a] == positions) //if exists, add one to that positions counter
                            {
                                exist = true;
                                quantity_tmp[a]++;
                            }

                        }
                        if (exist == false) //if doesn't exist, adds combination as new
                        {
                            positions_tmp.Add(positions);
                            quantity_tmp.Add(1);
                        }
                        words_tmp.Add(positions);

                    }
                }
                int group_count = 0; //How many words are in the biggest group
                int group_index = 0;//Group position

                //Looks for the group with more words and same choice position
                for (int i = 0; i < quantity_tmp.Count; i++)
                {

                    if (quantity_tmp[i] > group_count)//if the current group has more words, sets group count as the right amount
                    {
                        group_count = quantity_tmp[i];
                        group_index = i;
                    }
                }

                List<string> result_final = new List<string>();

                for (int i = 0; i < words_tmp.Count; i++)//adds to the temporal result list (result_tmp) all words with the biggest group
                {
                    if (words_tmp[i] == positions_tmp[group_index])
                    {
                        result_final.Add(result[i]);
                    }
                }
                result.Clear();
                for (int i = 0; i < result_final.Count; i++) //adds the the result_tmp to the official result list
                {
                    result.Add(result_final[i]);
                }
            }



            possible_words.Clear();
            dictionary.Clear();
            for (int i = 0; i < result.Count; i++)
            {
                dictionary.Add(result[i]);
                possible_words.Add(result[i]);
            }




        }
        static string Word()
        {
            //C:\Users\mayro\OneDrive\Escritorio\ArtificialIntelligence\PracticeAssessment_AI\dictionary.txt
            Random rd = new Random();

            string[] dictionary_tmp = File.ReadAllLines(textFile);
            dictionary.Clear();

            int word_length = rd.Next(4, 12);
            for (int i = 0; i < dictionary_tmp.Length; i++)
            {
                if (dictionary_tmp[i].Length == word_length)
                {
                    dictionary.Add(dictionary_tmp[i]);
                }

            }

            return dictionary[0];
        }

        static void GameEasy(string secret_word, int lifes, StringBuilder tape, List<char> letters_used)
        {

                int count_g = 0;

            do
            {
              

                int cell = 0;//starts the TM on first letter
                char read;
                char check;
                string choice;
                StringBuilder hearts = new StringBuilder();
                for (int i = 0; i < lifes; i++) //Shows the Lifes as hearts
                {

                    hearts.Append('♥');
                }

                string state = "START";
                Console.WriteLine("Lifes = " + hearts);
                Console.WriteLine(tape);

                do
                {
                    Console.WriteLine("Choose your letter");
                    choice = Console.ReadLine();
                } while (choice.Length != 1);


                Dictionary(dictionary, choice);
                secret_word = possible_words[random];



                do
                {

                    Console.Clear();
                    
                    read = tape[cell]; // user choice
                    check = secret_word[cell];

                    if (state == "START" && check == choice[0])
                    {
                        tape[cell] = choice[0];
                        count_g++;
                        cell++;
                        state = "NEXT";
                    }
                    else if (state == "START" && check != choice[0])
                    {
                        tape[cell] = tape[cell];
                        cell++;
                        state = "NEXT";
                    }
                    else if (state == "NEXT" && check == choice[0])
                    {
                        tape[cell] = choice[0];
                        count_g++;
                        if (cell == secret_word.Length - 1)
                        {
                            state = "HALT";
                            //Console.WriteLine(tape);
                        }
                        else
                        {
                            cell++;
                            state = "NEXT";
                        }

                    }
                    else if (state == "NEXT" && check != choice[0])
                    {
                        tape[cell] = tape[cell];

                        if (cell == secret_word.Length - 1)
                        {
                            state = "HALT";
                            //Console.WriteLine(tape);
                        }
                        else
                        {
                            cell++;
                            state = "NEXT";
                        }

                    }

                    //When a letter is multiple times in the string, count_g is added as many times as it is on the string
                    //Ex. When user chooses letter 'o' as guess, count_g = 1 in string "word", but count_g = 2 in string "wood"
                    // Console.WriteLine("count_g = " + count_g);


                } while (state != "HALT" && count_g != secret_word.Length);

                if (count_g == 0)
                {
                    lifes = lifes - 1; //reduces a life every time user does not guess
                }


                letters_used.Add(choice[0]);

                Console.WriteLine("Letters used: ");
                for (int i = 0; i < letters_used.Count; i++)
                {
                    Console.WriteLine(letters_used[i]);
                }
                //https://www.educative.io/answers/how-to-check-if-two-strings-are-equal-in-c-sharp
            } while (count_g != secret_word.Length && lifes != 0); //Ends the game when secret words is revealed or there are no more lifes

            Console.WriteLine("Lifes: " + lifes);
            if (lifes == 0)
            {
                //Console.WriteLine("Lifes = " + lifes);

                Console.WriteLine("Ooops! You run out of lifes! The word was: " + secret_word);
                Console.WriteLine("You lost!");
                Console.ReadKey();


            }
            else// if (tape.Equals(secret_word) != true)
            {
                Console.WriteLine("Congratulations!! The secret word was " + secret_word);
                Console.WriteLine("You won!");
                Console.ReadKey();
            }
        }

        static void GameDifficult(string secret_word, int lifes, StringBuilder tape, List<char> letters_used)
        {
            int count_g = 0;
            do
            {
             

                int cell = 0;//starts the TM on first letter
                char read;
                char check;
                string choice; 

                StringBuilder hearts = new StringBuilder();
                for (int i = 0; i < lifes; i++) //Shows the Lifes as hearts
                {

                    hearts.Append('♥');
                }

                string state = "START";
                Console.WriteLine("Lifes = " + hearts);
                Console.WriteLine(tape);

                do
                {
                    Console.WriteLine("Choose your letter");
                    choice = Console.ReadLine();
                } while (choice.Length != 1);

                Dictionary2(dictionary, choice);
                secret_word = possible_words[random];
               
                
                do
                {

                    Console.Clear();
                    
                    read = tape[cell]; // user choice
                    check = secret_word[cell];

                    if (state == "START" && check == choice[0])
                    {
                        tape[cell] = choice[0];
                        count_g++;
                        cell++;
                        state = "NEXT";
                    }
                    else if (state == "START" && check != choice[0])
                    {
                        tape[cell] = tape[cell];
                        cell++;
                        state = "NEXT";
                    }
                    else if (state == "NEXT" && check == choice[0])
                    {
                        tape[cell] = choice[0];
                        count_g++;
                        if (cell == secret_word.Length - 1)
                        {
                            state = "HALT";
                            //Console.WriteLine(tape);
                        }
                        else
                        {
                            cell++;
                            state = "NEXT";
                        }

                    }
                    else if (state == "NEXT" && check != choice[0])
                    {
                        tape[cell] = tape[cell];

                        if (cell == secret_word.Length - 1)
                        {
                            state = "HALT";
                            //Console.WriteLine(tape);
                        }
                        else
                        {
                            cell++;
                            state = "NEXT";
                        }

                    }

                    //When a letter is multiple times in the string, count_g is added as many times as it is on the string
                    //Ex. When user chooses letter 'o' as guess, count_g = 1 in string "word", but count_g = 2 in string "wood"
                    // Console.WriteLine("count_g = " + count_g);


                } while (state != "HALT");

                if (count_g == 0)
                {
                    lifes = lifes - 1; //reduces a life every time user does not guess
                }


                letters_used.Add(choice[0]);

                Console.WriteLine("Letters used: ");
                for (int i = 0; i < letters_used.Count; i++)
                {

                    Console.WriteLine(letters_used[i]);
                }
                //https://www.educative.io/answers/how-to-check-if-two-strings-are-equal-in-c-sharp
            } while (count_g != secret_word.Length && lifes != 0);//Ends the game when secret words is revealed or there are no more lifes

            Console.WriteLine("Lifes: " + lifes);
            if (lifes == 0)
            {
                //Console.WriteLine("Lifes = " + lifes);

                Console.WriteLine("Ooops! You run out of lifes! The word was: " + secret_word);
                Console.WriteLine("You lost!");
                Console.ReadKey();

            }
            else// if (tape.Equals(secret_word) != true)
            {
                Console.WriteLine("Congratulations!! The secret word was " + secret_word);
                Console.WriteLine("You won!");
                Console.ReadKey();
            }
        }
    }
}