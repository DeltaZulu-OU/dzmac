#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DZMACLib
{
    internal class Cache : IDisposable
    {
        private readonly string _databaseFile;
        private SQLiteConnection? _connection;
        private bool _disposedValue;
        private readonly Regex _pattern = new Regex("^[0-9A-F]{6}$");

        public int Count { get; private set; }

        public Vendor? this[int index] => GetByIndex(index);

        public Cache(string databaseFile)
        {
            _databaseFile = databaseFile;
            _connection = CreateConnection();
            CreateTableIfNotExists();
            UpdateCount();
        }

        public Vendor? this[string oui] => Get(oui);

        /// <summary>
        ///     Add an instance of Vendors to the database
        /// </summary>
        /// <param name="oui">The OUI for vendor</param>
        /// <param name="vendor">Vendor name</param>
        public void Add(string oui, string vendor)
        {
            Debug.WriteLine($"Updating database (OUI: {oui}, Vendor: {vendor})...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            command.CommandText = "INSERT INTO vendors (oui, vendor) VALUES($oui, $vendor);";
            command.Parameters.AddWithValue("$oui", oui);
            command.Parameters.AddWithValue("$vendor", vendor);
            command.ExecuteNonQuery();

            UpdateCount();
        }

        /// <summary>
        ///     Add a collection of Vendors to the database
        /// </summary>
        /// <param name="vendors">A collection of Vendor instances</param>
        public void AddRange(IEnumerable<Vendor> vendors)
        {
            Debug.WriteLine("Populating database...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using (var transaction = _connection.BeginTransaction())
            {
                using var command = _connection.CreateCommand();
                command.CommandText = "INSERT INTO vendors VALUES ($oui, $vendor)";
                var ouiParameter = command.CreateParameter();
                ouiParameter.ParameterName = "$oui";
                command.Parameters.Add(ouiParameter);

                var vendorParameter = command.CreateParameter();
                vendorParameter.ParameterName = "$vendor";
                command.Parameters.Add(vendorParameter);

                foreach (var record in vendors)
                {
                    ouiParameter.Value = record.Oui;
                    vendorParameter.Value = record.VendorName;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            UpdateCount();
        }

        /// <summary>
        ///     Get a vendor by OUI
        /// </summary>
        /// <param name="oui">IEEE assigned OUI</param>
        /// <returns>List of vendors matching the OUI</returns>
        /// <exception cref="ArgumentException">OUI should be 6 hexadecimal characters. If not, an exception is thrown.</exception>
        public Vendor? Get(string oui, bool useWildcard = false)
        {
            if (!_pattern.IsMatch(oui))
            {
                throw new ArgumentException(nameof(oui));
            }

            Debug.WriteLine($"Querying database (OUI: {oui})...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            command.CommandText = "SELECT oui,vendor FROM vendors WHERE oui LIKE $oui LIMIT 1";

            if (useWildcard)
            {
                var charArray = oui.ToCharArray();
                charArray[1] = '%';
                oui = new string(charArray);
            }
            command.Parameters.AddWithValue("$oui", oui);
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            var vendorOui = reader.GetString(0).Replace("\r", "");
            var vendorName = reader.GetString(1).Replace("\r", "");
            return new Vendor(vendorOui, vendorName);
        }

        /// <summary>
        ///     Queries the cache for all vendor records and converts into a collection of Vendor instances.
        /// </summary>
        /// <returns>A collection of Vendor instances</returns>
        public IEnumerable<Vendor> GetAll()
        {
            Debug.WriteLine("Querying database (ALL)...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            command.CommandText = "SELECT oui,vendor FROM vendors";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Vendor(reader.GetString(0).Replace("\r", ""), reader.GetString(1).Replace("\r", ""));
            }
        }

        public void Clear()
        {
            Debug.WriteLine("Clearing database...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using (var transaction = _connection.BeginTransaction())
            {
                using var command = _connection.CreateCommand();
                command.CommandText = "DELETE FROM vendors;";
                command.ExecuteNonQuery();
                transaction.Commit();
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            UpdateCount();
        }

        /// <summary>
        ///     Checks if the cache is empty.
        /// </summary>
        /// <returns>True if cache has no vendor records.</returns>
        public bool IsEmpty
        {
            get
            {
                Debug.WriteLine("Querying database if empty...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                command.CommandText = "SELECT COUNT(*) FROM vendors";
                using var reader = command.ExecuteReader();
                reader.Read();
                return reader.GetInt32(0) == 0;
            }
        }

        private SQLiteConnection CreateConnection()
        {
            Debug.WriteLine($"Creating database connection to local db: {_databaseFile}...");
            var conn = new SQLiteConnection($"Data Source={_databaseFile}; Version = 3; New = True; Compress = True;");
            conn.Open();
            return conn;
        }

        private void CreateTableIfNotExists()
        {
            Debug.WriteLine("Creating vendor table (if not exists)...");
            const string newTableCommand = "CREATE TABLE IF NOT EXISTS vendors (oui VARCHAR(6), vendor VARCHAR(128))";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            command.CommandText = newTableCommand;
            command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Get the item from database by an index
        /// </summary>
        /// <param name="index">Index to the item</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private Vendor? GetByIndex(int index)
        {
            Debug.WriteLine($"Querying database (index: {index})...");

            if (index > Count)
            {
                throw new IndexOutOfRangeException();
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            command.CommandText = "SELECT * FROM vendors LIMIT 1 OFFSET $offset";
            command.Parameters.AddWithValue("$offset", index);
            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            var oui = reader.GetString(0).Replace("\r", "");
            var vendorName = reader.GetString(1).Replace("\r", "");
            return new Vendor(oui, vendorName);
        }

        private void UpdateCount()
        {
            Debug.WriteLine("Querying database for record count...");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            using var command = _connection.CreateCommand();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            command.CommandText = "SELECT COUNT(*) FROM vendors";
            using var reader = command.ExecuteReader();
            reader.Read();
            Count = reader.GetInt32(0);
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _connection?.Close();
                    _connection?.Dispose();
                }

                _connection = null;
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}
