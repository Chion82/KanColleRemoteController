/*
    This source file is created by @Chion82
    for the purpose of exporting real-time game data.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grabacr07.KanColleWrapper.Models;

namespace Grabacr07.KanColleWrapper
{
    class FleetInfoUtils
    {
        static public Object GetShipInfo(Ship ship)
        {
            return new
            {
                AA = new
                {
                    currentValue = ship.AA.Current,
                    defaultValue = ship.AA.Default,
                    isMax = ship.AA.IsMax,
                    max = ship.AA.Max,
                    shortfall = ship.AA.Shortfall,
                    upgraded = ship.AA.Upgraded
                },
                armor = new
                {
                    currentValue = ship.Armer.Current,
                    defaultValue = ship.Armer.Default,
                    isMax = ship.Armer.IsMax,
                    max = ship.Armer.Max,
                    shortfall = ship.Armer.Shortfall,
                    upgraded = ship.Armer.Upgraded
                },
                bull = new
                {
                    currentValue = ship.Bull.Current,
                    max = ship.Bull.Maximum,
                    min = ship.Bull.Minimum
                },
                condition = ship.Condition,
                conditionType = ship.ConditionType.ToString(),
                equippedSlots = GetSlots(ship.EquippedSlots),
                exp = ship.Exp,
                expForNextLevel = ship.ExpForNextLevel,
                firePower = new
                {
                    currentValue = ship.Firepower.Current,
                    defaultValue = ship.Firepower.Default,
                    ismax = ship.Firepower.IsMax,
                    max = ship.Firepower.Max,
                    shortfall = ship.Firepower.Shortfall,
                    upgraded = ship.Firepower.Upgraded
                },
                fuel = new
                {
                    current = ship.Fuel.Current,
                    max = ship.Fuel.Maximum,
                    min = ship.Fuel.Minimum
                },
                HP = new
                {
                    current = ship.HP.Current,
                    max = ship.HP.Maximum,
                    min = ship.HP.Minimum
                },
                id = ship.Id,
                info = new
                {
                    HP = ship.Info.HP,
                    id = ship.Info.Id,
                    maxAA = ship.Info.MaxAA,
                    maxArmor = ship.Info.MaxArmer,
                   // maxASW = ship.Info.MaxASW,
                   // maxEvasion = ship.Info.MaxEvasion,
                    maxFirePower = ship.Info.MaxFirepower,
                   // maxLOS = ship.Info.MaxLOS,
                   // maxTorpedo = ship.Info.MaxTorpedo,
                    name = ship.Info.Name,
                   // nextRemodelingLevel = ship.Info.NextRemodelingLevel,
                    shipType = ship.Info.ShipType.Name,
                    //slots = ship.Info.Slots.ToString(),
                    speed = ship.Info.Speed.ToString()
                },
                isLocked = ship.IsLocked,
                isMaxModernized = ship.IsMaxModernized,
                level = ship.Level,
                luck = new
                {
                    currentValue = ship.Luck.Current,
                    defaultValue = ship.Luck.Default,
                    isMax = ship.Luck.IsMax,
                    max = ship.Luck.Max,
                    shortfall = ship.Luck.Shortfall,
                    upgraded = ship.Luck.Upgraded
                },
                sallyArea = ship.SallyArea,
                situation = ship.Situation.ToString(),
               // slot = GetSlots(ship.Slots),
                sortNumber = ship.SortNumber,
                torpedo = new
                {
                    currentValue = ship.Torpedo.Current,
                    defaultValue = ship.Torpedo.Default,
                    isMax = ship.Torpedo.IsMax,
                    max = ship.Torpedo.Max,
                    shortfall = ship.Torpedo.Shortfall
                },
                viewRange = ship.ViewRange
            };

        }

        static public Array GetShipArray(Ship[] ships)
        {
            int shipCount = ships.Length;
            Object[] shipObjs = new Object[shipCount];
            for (int i = 0; i < shipCount; i++)
            {
                shipObjs[i] = GetShipInfo(ships[i]);
            }
            return shipObjs;
        }

        static public Array GetSlots(ShipSlot[] shipSlot)
        {
            int slotCount = shipSlot.Length;
            Object[] slots = new Object[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                slots[i] = new
                {
                    current = shipSlot[i].Current,
                    equipped = shipSlot[i].Equipped,
                    item = new
                    {
                        id = shipSlot[i].Item.Id,
                        info = new
                        {
                            AA = shipSlot[i].Item.Info.AA,
                            categoryId = shipSlot[i].Item.Info.CategoryId,
                            id = shipSlot[i].Item.Info.Id,
                            isAirSuperiorityFighter = shipSlot[i].Item.Info.IsAirSuperiorityFighter,
                            isNumerable = shipSlot[i].Item.Info.IsNumerable,
                            name = shipSlot[i].Item.Info.Name,
                            type = shipSlot[i].Item.Info.Type.ToString()
                        },
                        level = shipSlot[i].Item.Level,
                        levelText = shipSlot[i].Item.LevelText,
                        nameWithLevel = shipSlot[i].Item.NameWithLevel
                    },
                    max = shipSlot[i].Maximum
                };
            }
            return slots;
        }

        static public Object GetExpeditionInfo(Expedition expedition)
        {
            if (expedition.Mission != null && expedition.Remaining != null && expedition.ReturnTime != null)
            {
                return new
                {
                    expeditionId = expedition.Id,
                    isInExecution = expedition.IsInExecution,
                    mission = new
                    {
                        missionId = expedition.Mission.Id,
                        details = expedition.Mission.Detail,
                        title = expedition.Mission.Title
                    },
                    remaining = expedition.Remaining.Value.TotalSeconds,
                    returnTime = DataExportUtils.GetUnixTimeStramp(expedition.ReturnTime.Value.LocalDateTime)
                };
            }
            else if (expedition.Mission != null && (expedition.Remaining == null || expedition.ReturnTime == null))
            {
                return new
                {
                    expeditionId = expedition.Id,
                    isInExecution = expedition.IsInExecution,
                    mission = new
                    {
                        missionId = expedition.Mission.Id,
                        details = expedition.Mission.Detail,
                        title = expedition.Mission.Title
                    },
                    remaining = 0,
                    returnTime = 0
                };
            }
            else
            {
                return new
                {
                    expeditionId = -1,
                    isInExecution = expedition.IsInExecution,
                    mission = new
                    {
                        missionId = -1,
                        details = "",
                        title = ""
                    },
                    remaining = 0,
                    returnTime = 0
                };
            }
        }

        static public Object GetFleetConditionInfo(FleetState fleetState)
        {
            if (fleetState.Condition.IsRejuvenating && fleetState.Condition.RejuvenateTime!=null && fleetState.Condition.Remaining!=null)
            { 
                return new
                {
                    airSuperiorityPotential = fleetState.AirSuperiorityPotential,
                    averageLevel = fleetState.AverageLevel,
                    condition = new
                    {
                        isEnabled = fleetState.Condition.IsEnabled,
                        isRejuvenating = fleetState.Condition.IsRejuvenating,
                        name = fleetState.Condition.Name,
                        rejuvenateTime = DataExportUtils.GetUnixTimeStramp(fleetState.Condition.RejuvenateTime.Value.LocalDateTime),
                        remaining = fleetState.Condition.Remaining.Value.TotalSeconds
                    },
                    isReady = fleetState.IsReady,
                    situation = fleetState.Situation.ToString(),
                    speed = fleetState.Speed.ToString(),
                    totalLevel = fleetState.TotalLevel,
                    viewRange = fleetState.ViewRange,
                    viewRangeCalcType = fleetState.ViewRangeCalcType
                };
            }
            else
            {
                return new
                {
                    airSuperiorityPotential = fleetState.AirSuperiorityPotential,
                    averageLevel = fleetState.AverageLevel,
                    condition = new
                    {
                        isEnabled = fleetState.Condition.IsEnabled,
                        isRejuvenating = fleetState.Condition.IsRejuvenating,
                        name = fleetState.Condition.Name,
                        rejuvenateTime = 0,
                        remaining = 0
                    },
                    isReady = fleetState.IsReady,
                    situation = fleetState.Situation.ToString(),
                    speed = fleetState.Speed.ToString(),
                    totalLevel = fleetState.TotalLevel,
                    viewRange = fleetState.ViewRange,
                    viewRangeCalcType = fleetState.ViewRangeCalcType
                };
            }
        }
        
    }
}
