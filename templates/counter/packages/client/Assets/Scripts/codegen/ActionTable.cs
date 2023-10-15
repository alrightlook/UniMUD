/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class ActionTable : IMudTable
    {
        public class ActionTableUpdate : RecordUpdate
        {
            public string? key;
            public string? Previouskey;
            public uint? action;
            public uint? Previousaction;
            public int? x;
            public int? Previousx;
            public int? y;
            public int? Previousy;
            public string? target;
            public string? Previoustarget;
        }

        public readonly static string ID = "Action";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public string? key;
        public uint? action;
        public int? x;
        public int? y;
        public string? target;

        public override Type TableType()
        {
            return typeof(ActionTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(ActionTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            ActionTable other = (ActionTable)obj;

            if (other == null)
            {
                return false;
            }
            if (key != other.key)
            {
                return false;
            }
            if (action != other.action)
            {
                return false;
            }
            if (x != other.x)
            {
                return false;
            }
            if (y != other.y)
            {
                return false;
            }
            if (target != other.target)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            key = (string)functionParameters[0];

            action = (uint)functionParameters[1];

            x = (int)functionParameters[2];

            y = (int)functionParameters[3];

            target = (string)functionParameters[4];
        }

        public static IObservable<RecordUpdate> GetUpdates<T>()
            where T : IMudTable, new()
        {
            IMudTable mudTable = (IMudTable)Activator.CreateInstance(typeof(T));

            return NetworkManager.Instance.sync.onUpdate
                .Where(update => update.Table.Name == ID)
                .Select(recordUpdate =>
                {
                    return mudTable.RecordUpdateToTyped(recordUpdate);
                });
        }

        public override RecordUpdate RecordUpdateToTyped(RecordUpdate recordUpdate)
        {
            var currentValue = recordUpdate.CurrentValue as Property;
            var previousValue = recordUpdate.PreviousValue as Property;

            return new ActionTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentValue = recordUpdate.CurrentValue,
                PreviousValue = recordUpdate.PreviousValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                key = (string)(currentValue?["key"] ?? null),
                action = (uint)(int)(currentValue?["action"] ?? null),
                x = (int)(currentValue?["x"] ?? null),
                y = (int)(currentValue?["y"] ?? null),
                target = (string)(currentValue?["target"] ?? null),
                Previouskey = (string)(previousValue?["key"] ?? null),
                Previousaction = (uint)(int)(previousValue?["action"] ?? null),
                Previousx = (int)(previousValue?["x"] ?? null),
                Previousy = (int)(previousValue?["y"] ?? null),
                Previoustarget = (string)(previousValue?["target"] ?? null),
            };
        }

        public override void RecordToTable(RxRecord record)
        {
            var table = record.RawValue;

            var keyValue = (string)table["key"];
            key = keyValue;
            var actionValue = (uint)table["action"];
            action = actionValue;
            var xValue = (int)table["x"];
            x = xValue;
            var yValue = (int)table["y"];
            y = yValue;
            var targetValue = (string)table["target"];
            target = targetValue;
        }
    }
}
