using Memory;
using System;
using System.Linq;

namespace DarkHoles.Model.MemoryUtilities
{
    public interface IDarkSoulsIIIMemoryUtilities
    {
        void OpenProcess();
        bool IsConnectedToProcess { get; }
        int GetCurrentHP();
        int GetMaxHP();
    }

    public class DarkSoulsIIIMemoryUtilities : IDarkSoulsIIIMemoryUtilities
    {
        public const string DarkSoulsIIExeName = "DarkSoulsIII.exe";
        public const string WorldChrManAob = "48 8B 1D ?? ?? ?? 04 48 8B F9 48 85 DB ?? ?? 8B 11 85 D2 ?? ?? 8D";

        public string? WorldChrMan { get; private set; }

        public bool IsConnectedToProcess { get; set; }

        private readonly Mem _mem;

        public DarkSoulsIIIMemoryUtilities()
        {
            _mem = new Mem();
        }

        public void OpenProcess()
        {
            IsConnectedToProcess = _mem.OpenProcess(DarkSoulsIIExeName);

            WorldChrMan = GetWorldChrManAddress();
        }

        private string GetWorldChrManAddress()
        {
            var aoBScanAddress = _mem.AoBScan(WorldChrManAob).Result.First();
            var instructionData = _mem.ReadBytes(aoBScanAddress.ToString("X"), 8);

            var disassembler = new SharpDisasm.Disassembler(
                instructionData,
                SharpDisasm.ArchitectureMode.x86_64, (ulong)aoBScanAddress);

            var disassembledInstruction = disassembler.Disassemble().First();

            // Instruction is: mov rbx, [rip+0x42bd577]
            // rip is a special register that always holds the memory address of the next instruction to execute in the program's code segment
            // PC is program counter
            // So the PC property for the instruction is same as rip (??)
            // So we can compute the value of the second operand (0x7FF629B5FDB8) by just effectively calculating that value
            var worldChrMan = disassembledInstruction.PC + (ulong)disassembledInstruction.Operands[1].Value;

            return worldChrMan.ToString("X");
        }

        private const string CurrentHPValueReference = "0x{0:WorldChrMan},0x80,0x1F90,0x18,0xD8";
        private const string MaxHPValueReference = "0x{WorldChrMan},0x80,0x1F90,0x18,0xE0";

        public int GetCurrentHP() => _mem.ReadInt($"0x{WorldChrMan},0x80,0x1F90,0x18,0xD8");
        public int GetMaxHP() => _mem.ReadInt($"0x{WorldChrMan},0x80,0x1F90,0x18,0xE0");

        static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
