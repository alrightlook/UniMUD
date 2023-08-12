#nullable enable
using Cysharp.Threading.Tasks;
using mud.Network.IStore;
using mud.Network.IStore.ContractDefinition;
using mud.Network.schemas;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using NLog;
using static mud.Network.schemas.Common;
using DecodeStore = mud.Network.schemas.DecodeStore;

namespace mud.Network
{
	public static partial class Sync
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		public static async UniTask<Types.NetworkTableUpdate?> EcsEventFromLog(FilterLog log,
				IStoreService store, bool lastEventInTx)
		{
			var eventSig = log.EventSignature().Replace("0x", "");
			var storeSetFieldSignature =
					store.ContractHandler.GetEvent<StoreSetFieldEventDTO>().EventABI.Sha3Signature;
			var storeSetRecordSignature =
					store.ContractHandler.GetEvent<StoreSetRecordEventDTO>().EventABI.Sha3Signature;
			var storeDeleteRecordSignature =
					store.ContractHandler.GetEvent<StoreDeleteRecordEventDTO>().EventABI.Sha3Signature;
			var storeEphemeralRecordSignature =
				store.ContractHandler.GetEvent<StoreEphemeralRecordEventDTO>().EventABI.Sha3Signature;

			var ecsEvent = new Types.NetworkTableUpdate
			{
				Type = Types.NetworkEvents.NetworkComponentUpdate,
				BlockNumber = log.BlockNumber.Value,
				TxHash = log.TransactionHash,
				LogIndex = log.LogIndex.Value,
				LastEventInTx = lastEventInTx
			};

			if (eventSig == storeSetRecordSignature)
			{
				var decoded = Event<StoreSetRecordEventDTO>.DecodeEvent(log);
				var tableId = TableId.FromBytes32(decoded.Event.Table);
				var component = tableId.ToString();
				var entity = EntityIdUtil.KeyTupleToEntityID(decoded.Event.Key);
				var entityTuple = EntityIdUtil.BytesToStringArray(decoded.Event.Key);
				var data = ByteArrayToHexString(decoded.Event.Data);

				var value = await DecodeStore.DecodeStoreSetRecord(store, tableId, entityTuple, data);
				Logger.Debug($"StoreSetRecord: {tableId}, {component}, {entity}");

				ecsEvent.Component = component;
				ecsEvent.Entity = new Types.EntityID { Value = entity };
				ecsEvent.Value = value;

				return ecsEvent;
			}
			
			if (eventSig == storeEphemeralRecordSignature)
			{
				var decoded = Event<StoreEphemeralRecordEventDTO>.DecodeEvent(log);
				var tableId = TableId.FromBytes32(decoded.Event.Table);
				var component = tableId.ToString();
				var entity = EntityIdUtil.KeyTupleToEntityID(decoded.Event.Key);
				var entityTuple = EntityIdUtil.BytesToStringArray(decoded.Event.Key);
				var data = ByteArrayToHexString(decoded.Event.Data);

				var value = await DecodeStore.DecodeStoreSetRecord(store, tableId, entityTuple, data);
				Logger.Debug($"StoreEphemeralRecord: {tableId}, {component}, {entity}");

				ecsEvent.Component = component;
				ecsEvent.Entity = new Types.EntityID { Value = entity };
				ecsEvent.Value = value;
				ecsEvent.Ephemeral = true;
			}

			if (eventSig == storeSetFieldSignature)
			{
				var decoded = Event<StoreSetFieldEventDTO>.DecodeEvent(log);
				var tableId = TableId.FromBytes32(decoded.Event.Table);
				var component = tableId.ToString();
				var entity = EntityIdUtil.KeyTupleToEntityID(decoded.Event.Key);
				var data = ByteArrayToHexString(decoded.Event.Data);

				var (schema, value, initialValue) = await DecodeStore.DecodeStoreSetField(store, tableId, decoded.Event.SchemaIndex, data);

				Logger.Debug($"StoreSetField: {tableId}, {component}, {entity}");

				ecsEvent.Component = component;
				ecsEvent.Entity = new Types.EntityID { Value = entity };
				ecsEvent.InitialValue = initialValue;
				ecsEvent.PartialValue = value;

				return ecsEvent;
			}

			if (eventSig == storeDeleteRecordSignature)
			{
				var decoded = Event<StoreDeleteRecordEventDTO>.DecodeEvent(log);
				var tableId = TableId.FromBytes32(decoded.Event.Table);
				var component = tableId.ToString();
				var entity = EntityIdUtil.KeyTupleToEntityID(decoded.Event.Key);

				ecsEvent.Component = component;
				ecsEvent.Entity = new Types.EntityID { Value = entity };
				ecsEvent.Value = null;
				Logger.Debug($"StoreDeleteRecord: {tableId}, {component}, {entity}");
				return ecsEvent;
			}
			
			return null;
		}
	}
}
