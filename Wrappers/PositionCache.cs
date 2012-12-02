using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Typhon.Common;

namespace iGuardian.Wrappers
{
    internal sealed class PositionCache
    {
        private readonly ReadOnlyCollection<CachedPosition> _cachedPositions;

        internal PositionCache(IEnumerable<CachedPosition> positions)
        {
            _cachedPositions = new ReadOnlyCollection<CachedPosition>(positions.ToList());
        }

        internal ReadOnlyCollection<CachedPosition> CachedPositions
        {
            get { return _cachedPositions; }
        }

        #region Nested type: CachedPosition

        internal class CachedPosition
        {
            internal float Radius { get; set; }
            internal Vector3 Position { get; set; }
        }

        #endregion
    }
}
