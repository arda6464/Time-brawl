﻿namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;

    public class LogicPurchaseDoubleCoinsCommand : Command
    {
        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 509;
        }
    }
}
