/* Autogenerated file. Manual edits will not be saved.*/

#nullable enable
using System;
using mud;
using UniRx;
using Property = System.Collections.Generic.Dictionary<string, object>;

namespace mudworld
{
    public class RowTable : IMudTable
    {
        public class RowTableUpdate : RecordUpdate
        {
            public int? value;
            public int? Previousvalue;
        }

        public readonly static string ID = "Row";
        public static RxTable Table
        {
            get { return NetworkManager.Instance.ds.store[ID]; }
        }

        public override string GetTableId()
        {
            return ID;
        }

        public int? value;

        public override Type TableType()
        {
            return typeof(RowTable);
        }

        public override Type TableUpdateType()
        {
            return typeof(RowTableUpdate);
        }

        public override bool Equals(object? obj)
        {
            RowTable other = (RowTable)obj;

            if (other == null)
            {
                return false;
            }
            if (value != other.value)
            {
                return false;
            }
            return true;
        }

        public override void SetValues(params object[] functionParameters)
        {
            value = (int)functionParameters[0];
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

            return new RowTableUpdate
            {
                Table = recordUpdate.Table,
                CurrentValue = recordUpdate.CurrentValue,
                PreviousValue = recordUpdate.PreviousValue,
                CurrentRecordKey = recordUpdate.CurrentRecordKey,
                PreviousRecordKey = recordUpdate.PreviousRecordKey,
                Type = recordUpdate.Type,
                value = (int)(currentValue?["value"] ?? null),
                Previousvalue = (int)(previousValue?["value"] ?? null),
            };
        }

        public override void RecordToTable(RxRecord record)
        {
            var table = record.RawValue;

            var valueValue = (int)table["value"];
            value = valueValue;
        }
    }
}
