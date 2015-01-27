/*
 * Jani Blackjack Analyser 0.8
 * Coded by Utku Sen / utkusen.com
 * Thanks for collaborating:
 * Gizem Ece Yılmaz
 * Yusuf Ceylan
 * Eda Karabiber
 * Cemre Tekpınar
 * 26/12/2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication3
{
    

    public partial class Form1 : Form
    {

        /*
         * declaring the variables
         * değişkenlerin tanımlanması
        */

        byte deckNumber; //number of decks to play - oynanacak deste sayısı
        int hitCounter = 0; //hit control variable - hit kontrol değikeni
        int playerTotal; //total integer value of cards in player's hand - oyuncunun elindeki kartların sayısal toplamı
        int dealerTotal; //total integer value of cards in dealer's hand - bankanın elindeki kartların sayısal toplamı
        int budget; //total money of player - oyuncunun toplam parası
        int betOnTable; //added bet to table - masaya konulan bahis
        int wonControl = 0; //control variable for winning - kazanç için kontrol değişkeni
        int numOfGoodCards; //number of cards which won't cause bust - batmaya yol açmayacak kart
        int numOfBadCards; //number of cards which will cause bust - batmaya yol açacak kart
        int row; //row number for basic strategy table - basic strategy tablosu için satır sayısı
        int column; //column number for basic strategy table - basic strategy tablosu için sütun sayısı
        Boolean isBet = false; //player added bets? - oyuncu bahis koydu mu?
        string[] newDeck;
        string[] playerHand = new string[5]; //player's hand in string array 
        string[] dealerHand = new string[5]; //dealer's hand in string array 
        string[,] strategyTable = new string[24, 11]; //basic strategy table in two dimensional array
        

        

        static Random _random = new Random();

        public Form1()
        {
            InitializeComponent();
            

        }

       
        public void newGame()
        {
            //gets the deck number - deck sayısını alır
            deckNumber = Convert.ToByte(Interaction.InputBox("Enter The Deck Number", "Deck Number", "3", 10, 10));
            //initial budget - başlangıç bütçesi
            budget = 1000;
            betOnTable = 0;
            label14.Text = budget.ToString();   //label14 dediğimiz şey ekrandaki bir yazı. parayı ora    
            string[] deck = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};
            //deck number*4 - kullanıcının girdiği deck sayısı *4
            newDeck = Enumerable.Repeat(deck, deckNumber*4).SelectMany(x => x).ToArray();
            //deck shuffles - deste karıştırılır
            Shuffle(newDeck);

            //görsel öğeler, algoritmadan bağımsız
            place.Enabled = true;
            pictureBox11.Visible = true;
            pictureBox12.Visible = true;
            pictureBox13.Visible = true;
            pictureBox14.Visible = true;
            label13.Visible = true;
            label18.Visible = true;
            label17.Visible = true;
            label14.Visible = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;
            
            

            MessageBox.Show(deckNumber + " decks ready to play.");

           
        }

        /*
         * Controls the deck if it's close to empty, if so shuffles again
         * Destenin bitmesini kontrol eder. Bitmeye yakınsa tekrar karıştırır.
         */ 
        public void deckControler()
        {
            if (newDeck.Length < 2)
            {
                string[] deck = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
                newDeck = Enumerable.Repeat(deck, deckNumber * 4).SelectMany(x => x).ToArray();
                Shuffle(newDeck);
                label10.Text = "Deck is Re-shuffled";
            } 
        }

        /*
         * Starts new hand
         * Yeni eli başlatır
         */ 
        public void newBet()
            
        {
            deckControler();
            if (isBet == true) { 
            playerTotal = 0;
            dealerTotal = 0;
            hitCounter = 0;
            stand.Enabled = true;
            hit.Enabled = true;
            Array.Clear(playerHand, 0, playerHand.Length);
            Array.Clear(dealerHand, 0, dealerHand.Length);

            clear();

            label15.Visible = true;
            label16.Visible = true;
            label19.Visible = true;
            label12.Text = "";
            stand.Enabled = true;
            hit.Enabled = true;
            doubleButton.Enabled = true;
            button2.Enabled = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBox5.Visible = true;
            pictureBox6.Visible = true;
           
            pictureBox2.Image = pictureBox10.Image;

            label5.Text = newDeck.Last();
            //card which is located top of the deck goes to player - destenin en üstündeki kart player'a verilir
            playerHand[0] = newDeck.Last();
            //given card deleted from deck array - verilen kart deste arrayinden silindi
            newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
            label6.Text = newDeck.Last();
            playerHand[1] = newDeck.Last();
            newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
            label1.Text = newDeck.Last();
            dealerHand[0] = newDeck.Last();
            newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
            label2.Text = newDeck.Last();
            dealerHand[1] = newDeck.Last();
            newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();

            label5.Visible = true;
            label6.Visible = true;
            label1.Visible = true;


            playerTotal += getScore(label5);
            playerTotal += getScore(label6);


            //If player got two A's in first hand, one of them is counted as 1 - İlk elden iki tane ace biri 1 olarak kabul edilir
            if (playerHand[0] == "A" && playerHand[1] == "A")
            {
                playerTotal = 12;
            }

            dealerTotal += getScore(label1);
            dealerTotal += getScore(label2);

            label12.Text = playerTotal.ToString();
            label11.Text = dealerTotal.ToString();
            
            label12.Visible = true;
            }

            else
            {
                MessageBox.Show("You didn't place your bet");
            }
            
        }

        //clears the table - masadaki değerleri temizler
        public void clear()
        {
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label2.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            wonControl = 0;
        }

        /*
         * Hit algorithm seperated with number of hits
         * Kart çekme algoritması çekilen kart sayısına göre ayrılmıştır
         */ 
        public void hitPlayer()
        {
            deckControler();
            //first hit - ilk hit
            if (hitCounter == 0)
            {
                label7.Text = newDeck.Last();
                playerHand[2] = newDeck.Last();
                newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();             
                label7.Visible = true;
                pictureBox7.Visible = true;
                playerTotal += getScore(label7);
                aceFound(hitCounter);
                label12.Text = playerTotal.ToString();
                
            }

            
            if (hitCounter == 1)
            {
                label8.Text = newDeck.Last();
                playerHand[3] = newDeck.Last();
                newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
                label8.Visible = true;
                pictureBox8.Visible = true;
                playerTotal += getScore(label8);
                aceFound(hitCounter);
                label12.Text = playerTotal.ToString(); 
               
            }

            
            if (hitCounter == 2)
            {
                label9.Text = newDeck.Last();
                playerHand[3] = newDeck.Last();
                newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
                label9.Visible = true;
                pictureBox9.Visible = true;
                playerTotal += getScore(label9);
                aceFound(hitCounter);
                label12.Text = playerTotal.ToString();
                
                
            }
            //if player passed 21 - oyuncu 21'i geçerse
            if (playerTotal > 21)
            {
                label10.Text = "Busted";
                label10.Visible = true;
                hit.Enabled = false;
                stand.Enabled = false;
                doubleButton.Enabled = false;
            }

            hitCounter++;
        }

        //calculates integer value of cards - kart değerlerini hesaplar
            public int getScore(Label n)
            {
           
              switch (n.Text)
               {
                case "J":
                case "Q":
                case "K":
                    return 10;
                case "A":
                    return 11;
                default:
                    return Convert.ToInt32(n.Text);
        }
              
}
            
        
        //if the player stands - oyuncu stand kararı verirse
        public void standPlayer()
        {
            stand.Enabled = false;
            hit.Enabled = false;
            doubleButton.Enabled = false;
            label11.Visible = true;
            int i = 2;
            label2.Visible = true;
            pictureBox2.Image = pictureBox1.Image;
            pictureBox2.SendToBack();
            label11.Visible = true;

            //if the dealer is in legal place - dealer olması gereken yerdeyse
            if (dealerTotal <= 21 && dealerTotal >= 17)
            {
                whoWins();
            }

            //if the dealer's total is under 17, need to take hit - dealer 17'den küçük oldukça kart çekmek zorunda
            else
            {
                while (dealerTotal < 17)
                {
                    dealerHand[i] = newDeck.Last();
                    newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
                    switch (dealerHand[i])
                    {
                        case "J": dealerTotal = dealerTotal + 10; break;
                        case "Q": dealerTotal = dealerTotal + 10; break;
                        case "K": dealerTotal = dealerTotal + 10; break;
                        case "A": dealerTotal = dealerTotal + 11; break;
                        default: dealerTotal = dealerTotal + Convert.ToInt32(dealerHand[i]); break;

                    }
                    aceFoundDealer(i);
                    label11.Text = dealerTotal.ToString();
                    i++;
                }
            }

            if (dealerHand[2] != null)
            {
                label3.Text = dealerHand[2];
                label3.Visible = true;
                pictureBox3.Visible = true;
            }

            if (dealerHand[3] != null)
            {
                label4.Text = dealerHand[3];
                label4.Visible = true;
                pictureBox4.Visible = true;
            }

            if (dealerHand[4] != null)
            {
                label5.Text = dealerHand[4];
                label5.Visible = true;
                pictureBox5.Visible = true;
            }

            whoWins();
        }

        /*
         * Double down can be done in only if playerTotal is 10 or 11
         * Oyuncunun kart toplamı 10 ya da 11 ise double down yapabilir
         */ 
        public void doubleDown()
        {
            if (playerTotal == 10 || playerTotal == 11)
            {
                if (hitCounter == 0)
                {
                    addBet(betOnTable);
                    label7.Text = newDeck.Last();
                    playerHand[2] = newDeck.Last();
                    newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
                    label7.Visible = true;
                    pictureBox7.Visible = true;
                    playerTotal += getScore(label7);
                    aceFound(hitCounter);
                    label12.Text = playerTotal.ToString();
                    standPlayer();

                }

                if (hitCounter == 1)
                {
                    addBet(betOnTable);
                    label8.Text = newDeck.Last();
                    playerHand[3] = newDeck.Last();
                    newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
                    label8.Visible = true;
                    pictureBox8.Visible = true;
                    playerTotal += getScore(label8);
                    aceFound(hitCounter);
                    label12.Text = playerTotal.ToString();
                    standPlayer();

                }

                if (hitCounter == 2)
                {
                    addBet(betOnTable);
                    label9.Text = newDeck.Last();
                    playerHand[4] = newDeck.Last();
                    newDeck = newDeck.Take(newDeck.Count() - 1).ToArray();
                    label9.Visible = true;
                    pictureBox9.Visible = true;
                    playerTotal += getScore(label9);
                    aceFound(hitCounter);
                    label12.Text = playerTotal.ToString();
                    standPlayer();

                }
            }
            else
            {
                MessageBox.Show("Double Down can be done in total 10/11");
            }
        }

        //gives the prize - kazancı aktarır
        public void wonPrize()
        {
            if (wonControl == 0) { 
            budget = budget + betOnTable * 2;
            
            }
            wonControl++; //her elde bir kere para aktarılması için kontrol eder.
            label14.Text = budget.ToString();
        }

         

        //checks who won - kimin kazandığını kontrol eder
        public void whoWins()
        {
            if (playerTotal == 21)
            {
                label10.Text = "Blackjack! You won:";
                wonPrize();

            }

            if (dealerTotal > 21)
            {
                label10.Text = "Dealer Busted!";
                wonPrize();
                
            }

            if (playerTotal == dealerTotal)
            {
                label10.Text = "Push";
                budget = budget + betOnTable;
                label14.Text = budget.ToString();
            }

            if (Math.Abs(playerTotal - 21) < Math.Abs(dealerTotal - 21) )
            {
                label10.Text = "You Won!";
                wonPrize();
            }

            if (Math.Abs(dealerTotal - 21) < Math.Abs(playerTotal - 21) && dealerTotal<=21)
            {
                label10.Text = "Dealer Won";
            }

            label10.Visible = true;
            bet.Enabled = false;
            
        }

        /*
         * If the player got Ace and his card total is greater than 11, 10 point erased from playerTotal
         * Ace ilk başta 11 değeriyle kabul edilir ama oyuncunun eli 11'den yüksekse 10 puan düşürülür ace'in değeri 1 olur
         */

        public void aceFound(int n)
        {
            if (playerHand[n + 2] == "A" && playerTotal >= 11)
            {
                playerTotal = playerTotal - 10;

            }
        }

        public void aceFoundDealer(int n)
        {
            if (dealerHand[n] == "A" && dealerTotal >= 11)
            {
                dealerTotal = dealerTotal - 10;
            }
        }

        //Fisher-Yates Shuffle Algorithm
        public static void Shuffle<T>(T[] array)
        {
            var random = _random;
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); 
                // Swap.
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }

        //Adding bet to table - masaya bahis ekler
        public void addBet(int n)
        {
            betOnTable += n;
            label18.Text = betOnTable.ToString();
            bet.Enabled = true;
            isBet = true;
            budget = budget - n;
            label14.Text = budget.ToString();
            if (budget <= 0)
            {
                MessageBox.Show("You don't have enough money!");
                newBet();

            }
        }

        //calculates integer value of cards - kart değerlerini hesaplar
        public int getCardValue(String card)
        {
            switch (card)
            {
                case "J": return 10;
                case "Q": return 10;
                case "K": return 10;
                case "A": return 1;
                default: return Convert.ToInt32(card);

            }
        }

       /*
        * Card counting algorithm, analyses the cards in deck. If a card will cause player's bust, it's counted as bad card
        * otherwise it's good card.
        * Kart sayma algoritması destenin içindeki kartları analiz eder. Eğer bir kart oyuncunun batmasına yol açacaksa kötü kart
        * değilse iyi kart olarak kabul edilir.
        */ 
        public void cardCounter()
        {
            numOfGoodCards = 0;
            numOfBadCards = 0;

            int remain = 21 - playerTotal;

            for (int i = 0; i < newDeck.Length; i++)
            {
                if (getCardValue(newDeck[i]) <= remain)
                {
                    numOfGoodCards++;
                }

                else
                {
                    numOfBadCards++;
                }
            }

            int goodProb = (numOfGoodCards * 100) / (numOfGoodCards + numOfBadCards);
            int badProb = 100 - goodProb;
            MessageBox.Show("Probablity of good hit: %" + goodProb + " bad hit: %" + badProb);
            

        }

        /*
         * Looks up to basic strategy table according to dealer's up card and player's card total
         * Banka'nın elindeki açık karta ve oyuncunun kart toplamına göre basic strategy tablosuna bakar
         */ 
        public void basicStrategy()
        {
            switch (dealerHand[0])
            {
                case "2": column = 0; break;
                case "3": column = 1; break;
                case "4": column = 2; break;
                case "5": column = 3; break;
                case "6": column = 4; break;
                case "7": column = 5; break;
                case "8": column = 6; break;
                case "9": column = 7; break;
                case "10": column = 8; break;
                case "J": column = 8; break;
                case "Q": column = 8; break;
                case "K": column = 8; break;
                case "A": column = 9; break;

            }

            if (playerHand[0] == "A")
            {

                switch (playerHand[1])
                {
                    case "2": row = 9; break;
                    case "3": row = 10; break;
                    case "4": row = 11; break;
                    case "5": row = 12; break;
                    case "6": row = 13; break;
                    case "7": row = 14; break;

                }

            }

            if (playerHand[1] == "A")
            {

                switch (playerHand[0])
                {
                    case "2": row = 9; break;
                    case "3": row = 10; break;
                    case "4": row = 11; break;
                    case "5": row = 12; break;
                    case "6": row = 13; break;
                    case "7": row = 14; break;

                }

            }

            else { 

            switch (playerTotal)
            {
                case 4: row = 0; break;
                case 5: row = 0; break;
                case 6: row = 0; break;
                case 7: row = 0; break;
                case 8: row = 0; break;
                case 9: row = 1; break;
                case 10: row = 2; break;
                case 11: row = 3; break;
                case 12: row = 4; break;
                case 13: row = 5; break;
                case 14: row = 6; break;
                case 15: row = 7; break;
                case 16: row = 8; break;
                case 17: row = 23; break;
                case 18: row = 23; break;
                case 19: row = 23; break;
                case 20: row = 23; break;
                case 21: row = 23; break;

            }
            }

            MessageBox.Show("Basic Strategy Says: " + strategyTable[row, column]);
        }

        //shows next card - sıradaki kartı gösterir
        public void nextCardCheat()
        {
            MessageBox.Show("Next Card is: " + newDeck.Last());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label15.BackColor = Color.Transparent;
            label16.BackColor = Color.Transparent;
            label11.BackColor = Color.Transparent;
            label12.BackColor = Color.Transparent;
            label10.BackColor = Color.Transparent;
            label14.BackColor = Color.Transparent;
            label17.BackColor = Color.Transparent;
            label13.BackColor = Color.Transparent;
            label18.BackColor = Color.Transparent;            
            label19.BackColor = Color.Transparent;
            label20.BackColor = Color.Transparent;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox2.BackColor = Color.Transparent;
            pictureBox3.BackColor = Color.Transparent;
            pictureBox4.BackColor = Color.Transparent;
            pictureBox5.BackColor = Color.Transparent;
            pictureBox6.BackColor = Color.Transparent;
            pictureBox7.BackColor = Color.Transparent;
            pictureBox8.BackColor = Color.Transparent;
            pictureBox11.BackColor = Color.Transparent;
            pictureBox12.BackColor = Color.Transparent;
            pictureBox13.BackColor = Color.Transparent;
            pictureBox14.BackColor = Color.Transparent;

            

            //Basic Strategy Table Implementation
            //Basic Strategy Tablosunun Aktarımı


            strategyTable[0, 0] = "hit";
            strategyTable[0, 1] = "hit";
            strategyTable[0, 2] = "hit";
            strategyTable[0, 3] = "hit";
            strategyTable[0, 4] = "hit";
            strategyTable[0, 5] = "hit";
            strategyTable[0, 6] = "hit";
            strategyTable[0, 7] = "hit";
            strategyTable[0, 8] = "hit";
            strategyTable[0, 9] = "hit";

            strategyTable[1, 0] = "hit";
            strategyTable[1, 1] = "double down";
            strategyTable[1, 2] = "double down";
            strategyTable[1, 3] = "double down";
            strategyTable[1, 4] = "double down";
            strategyTable[1, 5] = "hit";
            strategyTable[1, 6] = "hit";
            strategyTable[1, 7] = "hit";
            strategyTable[1, 8] = "hit";
            strategyTable[1, 9] = "hit";

            strategyTable[2, 0] = "double down";
            strategyTable[2, 1] = "double down";
            strategyTable[2, 2] = "double down";
            strategyTable[2, 3] = "double down";
            strategyTable[2, 4] = "double down";
            strategyTable[2, 5] = "double down";
            strategyTable[2, 6] = "double down";
            strategyTable[2, 7] = "double down";
            strategyTable[2, 8] = "hit";
            strategyTable[2, 9] = "hit";

            strategyTable[3, 0] = "double down";
            strategyTable[3, 1] = "double down";
            strategyTable[3, 2] = "double down";
            strategyTable[3, 3] = "double down";
            strategyTable[3, 4] = "double down";
            strategyTable[3, 5] = "double down";
            strategyTable[3, 6] = "double down";
            strategyTable[3, 7] = "double down";
            strategyTable[3, 8] = "double down";
            strategyTable[3, 9] = "hit";

            strategyTable[4, 0] = "hit";
            strategyTable[4, 1] = "hit";
            strategyTable[4, 2] = "stand";
            strategyTable[4, 3] = "stand";
            strategyTable[4, 4] = "stand";
            strategyTable[4, 5] = "hit";
            strategyTable[4, 6] = "hit";
            strategyTable[4, 7] = "hit";
            strategyTable[4, 8] = "hit";
            strategyTable[4, 9] = "hit";

            strategyTable[5, 0] = "stand";
            strategyTable[5, 1] = "stand";
            strategyTable[5, 2] = "stand";
            strategyTable[5, 3] = "stand";
            strategyTable[5, 4] = "stand";
            strategyTable[5, 5] = "hit";
            strategyTable[5, 6] = "hit";
            strategyTable[5, 7] = "hit";
            strategyTable[5, 8] = "hit";
            strategyTable[5, 9] = "hit";

            strategyTable[6, 0] = "stand";
            strategyTable[6, 1] = "stand";
            strategyTable[6, 2] = "stand";
            strategyTable[6, 3] = "stand";
            strategyTable[6, 4] = "stand";
            strategyTable[6, 5] = "hit";
            strategyTable[6, 6] = "hit";
            strategyTable[6, 7] = "hit";
            strategyTable[6, 8] = "hit";
            strategyTable[6, 9] = "hit";

            strategyTable[7, 0] = "stand";
            strategyTable[7, 1] = "stand";
            strategyTable[7, 2] = "stand";
            strategyTable[7, 3] = "stand";
            strategyTable[7, 4] = "stand";
            strategyTable[7, 5] = "hit";
            strategyTable[7, 6] = "hit";
            strategyTable[7, 7] = "hit";
            strategyTable[7, 8] = "surrender";
            strategyTable[7, 9] = "hit";

            strategyTable[8, 0] = "stand";
            strategyTable[8, 1] = "stand";
            strategyTable[8, 2] = "stand";
            strategyTable[8, 3] = "stand";
            strategyTable[8, 4] = "stand";
            strategyTable[8, 5] = "hit";
            strategyTable[8, 6] = "hit";
            strategyTable[8, 7] = "surrender";
            strategyTable[8, 8] = "surrender";
            strategyTable[8, 9] = "hit";

            strategyTable[9, 0] = "hit";
            strategyTable[9, 1] = "hit";
            strategyTable[9, 2] = "hit";
            strategyTable[9, 3] = "double down";
            strategyTable[9, 4] = "double down";
            strategyTable[9, 5] = "hit";
            strategyTable[9, 6] = "hit";
            strategyTable[9, 7] = "hit";
            strategyTable[9, 8] = "hit";
            strategyTable[9, 9] = "hit";

            strategyTable[10, 0] = "hit";
            strategyTable[10, 1] = "hit";
            strategyTable[10, 2] = "hit";
            strategyTable[10, 3] = "double down";
            strategyTable[10, 4] = "double down";
            strategyTable[10, 5] = "hit";
            strategyTable[10, 6] = "hit";
            strategyTable[10, 7] = "hit";
            strategyTable[10, 8] = "hit";
            strategyTable[10, 9] = "hit";

            strategyTable[11, 0] = "hit";
            strategyTable[11, 1] = "hit";
            strategyTable[11, 2] = "double down";
            strategyTable[11, 3] = "double down";
            strategyTable[11, 4] = "double down";
            strategyTable[11, 5] = "hit";
            strategyTable[11, 6] = "hit";
            strategyTable[11, 7] = "hit";
            strategyTable[11, 8] = "hit";
            strategyTable[11, 9] = "hit";

            strategyTable[12, 0] = "hit";
            strategyTable[12, 1] = "hit";
            strategyTable[12, 2] = "double down";
            strategyTable[12, 3] = "double down";
            strategyTable[12, 4] = "double down";
            strategyTable[12, 5] = "hit";
            strategyTable[12, 6] = "hit";
            strategyTable[12, 7] = "hit";
            strategyTable[12, 8] = "hit";
            strategyTable[12, 9] = "hit";

            strategyTable[13, 0] = "hit";
            strategyTable[13, 1] = "double down";
            strategyTable[13, 2] = "double down";
            strategyTable[13, 3] = "double down";
            strategyTable[13, 4] = "double down";
            strategyTable[13, 5] = "hit";
            strategyTable[13, 6] = "hit";
            strategyTable[13, 7] = "hit";
            strategyTable[13, 8] = "hit";
            strategyTable[13, 9] = "hit";

            strategyTable[14, 0] = "stand";
            strategyTable[14, 1] = "if possible double down else stand";
            strategyTable[14, 2] = "if possible double down else stand";
            strategyTable[14, 3] = "if possible double down else stand";
            strategyTable[14, 4] = "if possible double down else stand";
            strategyTable[14, 5] = "stand";
            strategyTable[14, 6] = "stand";
            strategyTable[14, 7] = "hit";
            strategyTable[14, 8] = "hit";
            strategyTable[14, 9] = "hit";

            strategyTable[15, 0] = "split";
            strategyTable[15, 1] = "split";
            strategyTable[15, 2] = "split";
            strategyTable[15, 3] = "split";
            strategyTable[15, 4] = "split";
            strategyTable[15, 5] = "split";
            strategyTable[15, 6] = "hit";
            strategyTable[15, 7] = "hit";
            strategyTable[15, 8] = "hit";
            strategyTable[15, 9] = "hit";

            strategyTable[16, 0] = "split";
            strategyTable[16, 1] = "split";
            strategyTable[16, 2] = "split";
            strategyTable[16, 3] = "split";
            strategyTable[16, 4] = "split";
            strategyTable[16, 5] = "split";
            strategyTable[16, 6] = "hit";
            strategyTable[16, 7] = "hit";
            strategyTable[16, 8] = "hit";
            strategyTable[16, 9] = "hit";

            strategyTable[17, 0] = "hit";
            strategyTable[17, 1] = "hit";
            strategyTable[17, 2] = "hit";
            strategyTable[17, 3] = "split";
            strategyTable[17, 4] = "split";
            strategyTable[17, 5] = "hit";
            strategyTable[17, 6] = "hit";
            strategyTable[17, 7] = "hit";
            strategyTable[17, 8] = "hit";
            strategyTable[17, 9] = "hit";

            strategyTable[18, 0] = "split";
            strategyTable[18, 1] = "split";
            strategyTable[18, 2] = "split";
            strategyTable[18, 3] = "split";
            strategyTable[18, 4] = "split";
            strategyTable[18, 5] = "hit";
            strategyTable[18, 6] = "hit";
            strategyTable[18, 7] = "hit";
            strategyTable[18, 8] = "hit";
            strategyTable[18, 9] = "hit";

            strategyTable[19, 0] = "split";
            strategyTable[19, 1] = "split";
            strategyTable[19, 2] = "split";
            strategyTable[19, 3] = "split";
            strategyTable[19, 4] = "split";
            strategyTable[19, 5] = "split";
            strategyTable[19, 6] = "hit";
            strategyTable[19, 7] = "hit";
            strategyTable[19, 8] = "hit";
            strategyTable[19, 9] = "hit";

            strategyTable[20, 0] = "split";
            strategyTable[20, 1] = "split";
            strategyTable[20, 2] = "split";
            strategyTable[20, 3] = "split";
            strategyTable[20, 4] = "split";
            strategyTable[20, 5] = "split";
            strategyTable[20, 6] = "split";
            strategyTable[20, 7] = "split";
            strategyTable[20, 8] = "split";
            strategyTable[20, 9] = "split";

            strategyTable[21, 0] = "split";
            strategyTable[21, 1] = "split";
            strategyTable[21, 2] = "split";
            strategyTable[21, 3] = "split";
            strategyTable[21, 4] = "split";
            strategyTable[21, 5] = "stand";
            strategyTable[21, 6] = "split";
            strategyTable[21, 7] = "split";
            strategyTable[21, 8] = "stand";
            strategyTable[21, 9] = "stand";


            strategyTable[23, 0] = "Stand";
            strategyTable[23, 1] = "Stand";
            strategyTable[23, 2] = "Stand";
            strategyTable[23, 3] = "Stand";
            strategyTable[23, 4] = "Stand";
            strategyTable[23, 5] = "Stand";
            strategyTable[23, 6] = "Stand";
            strategyTable[23, 7] = "Stand";
            strategyTable[23, 8] = "Stand";
            strategyTable[23, 9] = "Stand";

            

        }

        /*
         * If player presses "enter" button cheat code form appears, if he enters correct code cheat menu activates
         * "enter" tuşuna basınca hile kodu formu açılır, doğru hileyi girerse hile menüsü açılır.
         */ 
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData){
        KeyEventArgs e = new KeyEventArgs(keyData);
        if (e.KeyCode == Keys.Enter)
        {
            String cheat = Convert.ToString(Interaction.InputBox("Enter Cheat Code", "Code", " ", 10, 10));
            if (cheat == "IKnowWhatIamDoing")
            {
                MessageBox.Show("Cheat Activated");
                button6.Visible = true;
                label20.Visible = true;
            }
            else
            {
                MessageBox.Show("Wrong Code");
            }
            return true; 
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }




        public void button1_Click(object sender, EventArgs e)
        {
            
            
            newGame();
        }

        private void hit_Click(object sender, EventArgs e)
        {
            hitPlayer();
            button2.Enabled = false;
        }

        private void stand_Click(object sender, EventArgs e)
        {
            standPlayer();
        }

        private void bet_Click(object sender, EventArgs e)
        {
            newBet();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            addBet(10);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            addBet(25);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            addBet(50);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            addBet(100);
        }

        private void place_Click(object sender, EventArgs e)
        {
            deckControler();
            isBet = false;
            betOnTable = 0;
            label18.Text = betOnTable.ToString();
            clear();
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
           
        }

        private void doubleButton_Click(object sender, EventArgs e)
        {
            doubleDown();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cardCounter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            basicStrategy();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            nextCardCheat();
        }


    }
}


