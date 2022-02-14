using Assets.Scripts.Modules.Units;
using Atropos.Core;
using Bolt;
using Ludiq;
using System.Collections.Generic;
using System.Linq;
using NetContainer = Assets.Scripts.Modules.Networking.Realisation.Container;
using Unit = Bolt.Unit;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Container")]
    [UnitCategory("Atropos/Quests")]
    public class Container : Unit
    {
        [Inspectable , UnitHeaderInspectable("Asset")] public ItemAsset asset;


        protected override void Definition()
        {
            ValueInput value = ValueInput<NetContainer>("Container");

            ValueOutput<IEnumerable<IAsset>>("Assets", flow => GetCollection(flow, value));
            if (asset == null) return;
            ValueOutput("Contains", flow => GetCollection(flow, value).Any(EqualsAsset));

            ControlOutput output = ControlOutput("Next");

            ControlInput("Add Asset", flow =>
            {
                flow.GetValue<NetContainer>(value)
                    .AddNewItems((Dictionary<string,string>)asset.Values);
                return output;
            });

            ControlInput("Remove Asset", flow =>
            {
                NetContainer container = flow.GetValue<NetContainer>(value);
                IAsset item = ExtensionItem.Get(container)
                                          .Where(EqualsAsset)
                                          .FirstOrDefault();
                if (item != null)
                    container.RemoveItem(ExtensionItem.GetItemRef(item));
                return output;
            });
        }

        private bool EqualsAsset(IAsset x) 
            => x.Values["GUID"] == asset.Reference.AssetGUID;

        private static UniRx.ReactiveCollection<IAsset> GetCollection(Flow flow, ValueInput value) 
            => ExtensionItem.Get(flow.GetValue<NetContainer>(value));
    }
}
