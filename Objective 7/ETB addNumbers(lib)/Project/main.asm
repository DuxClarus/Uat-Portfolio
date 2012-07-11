TITLE MASM Assemble Lib	(main.asm)

; Description:	Assemble Lib 

INCLUDE Irvine32.inc

.data
message BYTE "Result: ", 0

.code
addNumbers PROC C,
			first:DWORD,
			second:DWORD
	call DumpRegs
	mov EDX, OFFSET message
	call WriteString
	call Crlf
	call WaitMsg
	mov EAX, first
	add EAX, second
	ret
addNumbers ENDP

END addNumbers