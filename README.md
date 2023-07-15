This is a version of the popular game Hangman, but in this game, the computer can cheat and change the word while the 'Human' user tries to guess the right word.

There are two different difficulties, each of them with a different algorithm.
The easy difficulty, after getting player’s letter, it loops through all the words and sorts them in
“sub-groups” (or word family) depending on how many times is that letter in the word. Then
finds the sub-group with more words on it and selects it as main. Then chooses a random word
to find the location of the letter on it (if it is in the word at all). Then loops again through the
words to get only the words that match the letter position. Finally, this is set as the new
dictionary group and starts the process again from the beginning

The hard difficulty loops through the initial words looking for the words that has fewer times
the letter chosen by the user. This makes highly likely that the user will fail the first three or
four attempts, as there are usually enough number of words without any of the first letters
chosen.
Now, until this point both difficulties offer a similar result, because even though they have
different algorithms, the easy difficulty find the sub-group with more words, and at the
beginning the group that will have more words is the one without the chosen letter (except a
few exceptions). But this is the point that proves why this algorithm has been chosen for the
difficult level and not the one in the easy one. After all the possibilities without the letter have
run out and the program can only find words with at least once the letter, instead of choosing
a random word to find the letter positions, the program first goes through all the positions
possibilities for each word and finds which “word family” will have the biggest number of

words. And then starts again. This way the programs succeeds at two different -but related-
goals:

1- Focuses on minimising the positive results for the player as it will try first to make
it lose a life (finding a word without the letter), and if this is not possible then it
will find the smaller number of times the letter is on the words + increasing the
possible words where the algorithm can keep working with.
2- Maximises the winning results for the machine, as the bigger is the number of
words we can use to cheat, the more possible letter combinations will be
available, and the human player will have to risk more lives to find the word.
