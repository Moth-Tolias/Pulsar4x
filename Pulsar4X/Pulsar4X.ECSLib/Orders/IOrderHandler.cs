﻿using System;

namespace Pulsar4X.ECSLib
{
    public interface IOrderHandler
    {
        Game Game { get; }

        void HandleOrder(EntityCommand entityCommand);
    }


    internal class StandAloneOrderHandler:IOrderHandler
    {
        internal StandAloneOrderHandler(Game game)
        {
            Game = game;
            game.OrderHandler = this;
        }

        public Game Game { get; private set; }

        public void HandleOrder(EntityCommand entityCommand)
        {
            if (entityCommand.IsValidCommand(Game))
            {
                if (entityCommand.UseActionLanes)
                {
                    var orderableDB = entityCommand.EntityCommanding.GetDataBlob<OrderableDB>();
                    orderableDB.AddCommandToList(entityCommand);
                    orderableDB.ProcessOrderList();
                }
                else
                {
                    if(entityCommand.ActionOnDate == null)// new DateTime())
                        entityCommand.ActionCommand();
                    else
                    {
                        entityCommand.EntityCommanding.Manager.ManagerSubpulses.AddEntityInterupt(entityCommand.ActionOnDate, nameof(OrderableProcessor), entityCommand.EntityCommanding);
                    }
                }
            }                            
        }
    }

}
