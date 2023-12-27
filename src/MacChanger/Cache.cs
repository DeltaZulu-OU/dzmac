using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MacChanger
{
    internal class Cache : IDisposable
    {
        private readonly string _databaseFile;
        private SQLiteConnection _connection;
        private bool _disposedValue;
        private readonly Regex _pattern = new Regex("[0-9A-F]{6}");

        public int Count { get; private set; }

        public Vendor this[int index] => GetByIndex(index);

        public Cache(string databaseFile)
        {
            _databaseFile = databaseFile;
            _connection = CreateConnection();
            CreateTableIfNotExists();
            Count = UpdateCount();
        }

        public IEnumerable<Vendor> this[string oui] => Get(oui);

        public void Add(string oui, string vendor)
        {
            Debug.WriteLine($"Updating database (OUI: {oui}, Vendor: {vendor})...");
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO vendors (oui, value) VALUES($oui, $vendor);";
            command.Parameters.AddWithValue("$oui", oui);
            command.Parameters.AddWithValue("$vendor", vendor);
            command.ExecuteNonQuery();

            Count = UpdateCount();
        }

        public void AddRange(IEnumerable<Vendor> vendors)
        {
            Debug.WriteLine("Populating database...");
            using (var transaction = _connection.BeginTransaction())
            {
                var command = _connection.CreateCommand();
                command.CommandText = "INSERT INTO vendors VALUES ($oui, $vendor)";

                foreach (var record in vendors)
                {
                    command.Parameters.AddWithValue("$oui", record.Oui);
                    command.Parameters.AddWithValue("$vendor", record.VendorName);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }

            Count = UpdateCount();
        }

        /// <summary>
        ///     Get a vendor by OUI
        /// </summary>
        /// <param name="oui">IEEE assigned OUI</param>
        /// <returns>List of vendors matching the OUI</returns>
        /// <exception cref="ArgumentException">OUI should be 6 hexadecimal characters. If not, an exception is thrown.</exception>
        public IEnumerable<Vendor> Get(string oui)
        {
            if (!_pattern.IsMatch(oui))
            {
                throw new ArgumentException(nameof(oui));
            }

            Debug.WriteLine($"Querying database (OUI: {oui})...");
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT vendor FROM vendors WHERE oui LIKE $oui";
            command.Parameters.AddWithValue("$oui", oui);
            var reader = command.ExecuteReader();

            var vs = new List<Vendor>();
            while (reader.Read())
            {
                var vendorName = reader.GetString(0).Replace("\r", "");
                vs.Add(new Vendor(oui, vendorName));
            }
            return vs.AsReadOnly();
        }

        public IEnumerable<Vendor> GetAll()
        {
            Debug.WriteLine("Querying database (ALL)...");
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT oui,vendor FROM vendors";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Vendor(reader.GetString(0).Replace("\r", ""), reader.GetString(1).Replace("\r", ""));
            }
        }

        public bool IsEmpty()
        {
            Debug.WriteLine("Querying database if empty...");
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM vendors";
            var reader = command.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0) == 0;
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
            var command = _connection.CreateCommand();
            command.CommandText = newTableCommand;
            command.ExecuteNonQuery();
        }

        private Vendor GetByIndex(int index)
        {
            Debug.WriteLine($"Querying database (index: {index})...");

            if(index > Count)
            {
                throw new IndexOutOfRangeException();
            }

            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM vendors LIMIT 1 OFFSET $offset";
            command.Parameters.AddWithValue("$offset", index);
            var reader = command.ExecuteReader();
            reader.Read();
            var oui = reader.GetString(0).Replace("\r", "");
            var vendorName = reader.GetString(1).Replace("\r", "");
            return new Vendor(oui, vendorName);
        }

        private int UpdateCount()
        {
            Debug.WriteLine("Querying database for record count...");
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM vendors";
            var reader = command.ExecuteReader();
            reader.Read();
            var count = reader.GetInt32(0);

            return count;
        }

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
    }
}
