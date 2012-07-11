TITLE MASM Template						(main.asm)

; Description:	C lib integration

INCLUDE Irvine32.inc
INCLUDELIB addNumbers.lib

addNumbers PROTO C,
		 first:DWORD,
		 second:DWORD

.data
message BYTE "Result", 0

.code
main PROC
	invoke addNumbers, 10, 20
	call DumpRegs
	mov EDX, OFFSET message
	call WaitMsg
	exit
main ENDP

END main