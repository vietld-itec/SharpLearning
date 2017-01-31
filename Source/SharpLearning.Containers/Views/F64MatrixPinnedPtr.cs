﻿using SharpLearning.Containers.Matrices;
using System;
using System.Runtime.InteropServices;

namespace SharpLearning.Containers.Views
{
    /// <summary>
    /// Pinned pointer to F64Matrix. Proper disposal required. Preferably use this in a Using statement
    /// 
    /// Using(var pinned = matrix.GetPinnedPointer())
    /// {
    ///     var view = pinned.View();
    /// }
    /// 
    /// </summary>
    public unsafe struct F64MatrixPinnedPtr : IDisposable
    {
        readonly GCHandle m_handle;
        double* m_ptr;
        readonly int m_rows;
        readonly int m_cols;

        /// <summary>
        /// Pinnes the provided F64Matrix
        /// </summary>
        /// <param name="matrix"></param>
        public F64MatrixPinnedPtr(F64Matrix matrix)
        {
            if (matrix == null) { throw new ArgumentNullException("matrix"); }
            
            var data = matrix.Data();
            m_handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            m_ptr = (double*)m_handle.AddrOfPinnedObject().ToPointer();
            m_rows = matrix.GetNumberOfRows();
            m_cols = matrix.GetNumberOfColumns();
        }

        /// <summary>
        /// Creates a view over the pinned F64Matrix
        /// </summary>
        /// <returns></returns>
        public F64MatrixView View()
        { 
            return new F64MatrixView(m_ptr, m_rows, m_cols); 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (m_ptr != null)
            {
                m_ptr = null;
                m_handle.Free();
            }
        }
    }
}
