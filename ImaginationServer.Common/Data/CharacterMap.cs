using FluentNHibernate.Mapping;

namespace ImaginationServer.Common.Data
{
    public class CharacterMap : ClassMap<Character>
    {
        public CharacterMap()
        {
            Id(x => x.Id);

            Map(x => x.Owner);

            Map(x => x.GmLevel);
            Map(x => x.Reputation);

            Map(x => x.Name);
            Map(x => x.UnapprovedName);
            Map(x => x.NameRejected);
            Map(x => x.FreeToPlay);

            Map(x => x.ShirtColor);
            Map(x => x.ShirtStyle);
            Map(x => x.PantsColor);
            Map(x => x.HairStyle);
            Map(x => x.HairColor);
            Map(x => x.Lh);
            Map(x => x.Rh);
            Map(x => x.Eyebrows);
            Map(x => x.Eyes);
            Map(x => x.Mouth);

            HasMany(x => x.Items).Cascade.All();
            Map(x => x.Level);
            HasMany(x => x.Missions).Element("Mission").Cascade.AllDeleteOrphan();

            Map(x => x.LastZoneId);
            Map(x => x.MapInstance);
            Map(x => x.MapClone);
            Map(x => x.Position);
        }
    }
}