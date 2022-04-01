@startuml
skinparam shadowing false

actor User as u
participant UI as ui
participant ProfileService as p
participant JobService as j
participant OfferService as o
participant ContractService as c
participant ReviewService as r

u -> ui
activate ui

ui -> p: ValidateUser(email,password)
activate p

ui <-- p: sessionToken
deactivate p

u <-- ui
u -> ui

ui -> j: ListJobs(filter)
activate j

ui <-- j: jobs
deactivate j

u <-- ui
u -> ui

ui -> o: CreateOffer(offer)
activate o

ui <-- o: offer
deactivate o

u <-- ui
u -> ui

ui -> c: ListContracts(userId)
activate c

ui <-- c: contracts

u <-- ui
u -> ui

ui -> c: ConcludeContract(id)

ui <-- c: contract
deactivate c

u <-- ui
u -> ui

ui -> r: CreateReview(Review)
activate r
ui <-- r: review
deactivate r

u <-- ui
deactivate ui

@enduml