TITLE MASM Template						(main.asm)

; Description:	Simple Arithmetic 
;myMessage BYTE "MASM program example",0dh,0ah,0 -0dh(13)Carriage Return and 0ah(10)Line Feed
;number1 DWORD	1000
;number2 DWORD	2000
;result DWORD	? ;"?" doesnt initialize 
;opcodeMask	WORD 111000000000b / 000 ap code - 0 indirect - 0 page zero - 0000000 address

INCLUDE Irvine32.inc

.data
instruction	WORD 110100101011b
opcode		WORD 0
indirectBit	WORD 0
pageZeroBit	WORD 0
address		WORD 0
opcodeShift	EQU	9
indirectShift	EQU	8
PageZeroShift	EQU	7
indirectMask	EQU	100000000b
pageZeroMask	EQU	10000000b
addressMask	EQU	1111111b
opcodeMessage	BYTE "Opcode: ", 0
indirectBitMessage	BYTE	"IndirectBit: ", 0
pageZeroBitMessage	BYTE "PageZeroBit: ", 0
addressMessage	BYTE "AddressL ", 0

.code
main PROC
	mov EAX, 0
	mov AX, instruction
	call extractOpcode
	call extractIndirectBit
	call extractPageZeroBit
	call extractAddress
	mov EDX, OFFSET opcodeMessage
	call WriteString
	mov EAX, 0
	mov AX, opcode
	mov EBX, 2
	call WriteBinB
	call Crlf

	Call WaitMsg
	exit
main ENDP

extractOpcode PROC uses EAX
	shr AX, opcodeShift
	mov opcode, AX
	ret	
extractOpcode ENDP

extractIndirectBit PROC uses EAX
	and AX, indirectMask
	shr AX, indirectShift
	mov indirectBit, AX
	ret
extractIndirectBit ENDP

extractPageZeroBit PROC uses EAX
	and AX, PageZeroMask
	shr AX, PageZeroShift
	mov PageZeroBit, AX
	ret
extractPageZeroBit ENDP

extractAddress PROC uses EAX
	and AX, addressMask
	mov address, AX
	ret
extractAddress ENDP

END main



; 000 – AND – AND the memory operand with AC.
; 001 – TAD – Two's complement ADd the memory operand to <L,AC> (a 13 bit signed value).
; 010 – ISZ – Increment the memory operand and Skip next instruction if result is Zero.
; 011 – DCA – Deposit AC into the memory operand and Clear AC.
; 100 – JMS – JuMp to Subroutine (storing return address in first word of subroutine!).
; 101 – JMP – JuMP.
; 110 – IOT – Input/Output Transfer (see below).
; 111 – OPR – microcoded OPeRations (see below).