using System;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace AdamsCodeChallange.ConsoleApp
{
    public class Neo4JDriver: IDisposable
    {
        private readonly IDriver _driver;
        private bool _disposed = false;

        ~Neo4JDriver() => Dispose(false);

        public Neo4JDriver(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }


        public async Task<bool> WordExists(string word)
        {
            var query = @"
                     MATCH(w:Word)
                     WHERE w.name = $word
                     RETURN w.name";


            var session = _driver.AsyncSession();

            try
            {
                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new { word });
                    return (await result.ToListAsync());
                });
                if (readResults.Count == 1)
                {
                    return true;
                }
                return false;

            }
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - ${ex}");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }


        public async Task CompareWords(string word1, string word2)
        {
            var query = @"
                      MATCH (w1:Word {name: $word1}),
                            (w2:Word {name: $word2}),
                            p = allShortestPaths((w1)-[:ONE_OFF*]->(w2))
                      RETURN p";

            var session = _driver.AsyncSession();
            try
            {

                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new { word1, word2 });
                    return (await result.ToListAsync());
                });

                Console.WriteLine($"Words have {(readResults.Count > 0 ? readResults.Count : "no")} paths");
                Console.WriteLine();
                Console.WriteLine();
                var totalPathsCount = 1;
                foreach (var result in readResults)
                {
                    if (readResults.Count > 1)
                    {
                        Console.WriteLine($"Path {totalPathsCount}");
                        Console.WriteLine();
                    }
                    var nodes = result.Values.First().Value.As<IPath>().Nodes.ToList();
                    var relationships = result.Values.First().Value.As<IPath>().Relationships.ToList();
                    Console.WriteLine($"Word can be changed in {relationships.Count} changes");
                    var index = 0;
                    foreach (var relationship in relationships)
                    {
                        Console.WriteLine($"Change the {nodes[index].Properties["name"].As<string>().ToCharArray()[relationship.Properties["indexToChange"].As<int>()]} at index {relationship.Properties["indexToChange"]} in '{nodes[index].Properties["name"].As<string>()}' to a {relationship.Properties["LetterToChangeTo"]} to get {nodes[index + 1].Properties["name"]}");
                        index++;
                    }
                    totalPathsCount++;
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            // Capture any errors along with the query and data for traceability
            catch (Neo4jException ex)
            {
                Console.WriteLine($"{query} - {ex}");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _driver?.Dispose();
            }

            _disposed = true;
        }
    }
}
