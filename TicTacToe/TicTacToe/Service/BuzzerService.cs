﻿using Explorer700Library;

namespace TicTacToe.Service
{
    public class BuzzerService
    {
        private readonly Explorer700 explorer;

        public BuzzerService(Explorer700 explorer) 
        {
            this.explorer = explorer;
        }

        public void ItsBuzzinTime()
        {
            this.explorer.Buzzer.Beep(1000);
        }
    }
}
