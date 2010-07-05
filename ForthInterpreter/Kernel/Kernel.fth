\ ***************************
\ Math Operations
\ ***************************

: within?       \ n1 n2 n3 -- flag
  1+ within ;


\ ***************************
\ Memory Operations
\ ***************************

: ,             \ x --
  here cell allot ! ;

: c,            \ x --
  here 1 allot c! ;

: erase         \ a-addr u --
  0 fill ;


\ ***************************
\ Variables
\ ***************************

: constant      \ x "<spaces>name" -- ; Exec: -- x
  value ;

: ->
  to ;

: array         \ size -- ; [child] n -- addr
  create
    cell *
    here over erase
    allot
  does>
    swap cell * + ;


\ ***************************
\ Dev Environment and Console
\ ***************************

: ?             \ a-addr --
  @ . ;
